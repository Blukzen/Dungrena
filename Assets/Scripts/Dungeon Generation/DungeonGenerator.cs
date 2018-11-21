using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public static string TAG = "[DungeonGenerator] ";

    public int roomHeight;
    public int roomWidth;
    public int roomSpacing;
    public int roomConnectionWidth;
    public int roomDifficulty;
    public int dungeonSize;

    public SpawnList spawnList;
    public RoomSet roomSet;

    public RuleTile wallTile;
    public WeightedRandomTile floorTile;
    public Tile barrierTile;

    public AbstractWeapon[] starterWeapons;

    public GameEvent GeneratedEvent;

    private Bounds dungeonBounds;
    private List<AbstractDungeonRoom> dungeonRooms = new List<AbstractDungeonRoom>();
    public List<AbstractDungeonRoom> DungeonRooms
    {
        get { return dungeonRooms; }
    }

    private DungeonSpawnRoom spawnRoom;
    public DungeonSpawnRoom SpawnRoom { get { return spawnRoom; } }
    private DungeonShopRoom shopRoom;
    public DungeonShopRoom ShopRoom { get { return shopRoom; } }
    private DungeonBossRoom bossRoom;
    public DungeonBossRoom BossRoom { get { return bossRoom; } }
    private GameObject enemies;
    public GameObject Enemies { get { return enemies; } }

    private RoomType[,] dungeonMaze;
    public RoomType[,] GetMaze()
    {
        return dungeonMaze;
    }

    private Tilemap walls;
    public Tilemap WallTilemap { get { return walls; } }
    private Tilemap floor;
    public Tilemap FloorTilemap { get { return floor; } }
    private Tilemap objects;
    public Tilemap ObjectTilemap { get { return objects; } }
    private Tilemap barriers;
    public Tilemap BarrierTilemap { get { return barriers; } }
    private TilemapRenderer wallRenderer;
    private TilemapRenderer floorRenderer;
    private TilemapRenderer objectsRenderer;

    private Coroutine generatingMaze;
    private float loadingProgress = 0;
    private float mazeGenProgress = 0;
    private float roomGenProgress = 0;
    private float pathfindingProgress = 0;
    private float enemySpawnProgress = 0;

    // Loading states
    private bool loadedMaze = false;
    private bool loadingMaze = false;
    private bool loadedRooms = false;
    private bool loadingRooms = false;
    private bool loadedPathfinding = false;
    private bool loadingPathfinding = false;
    private bool loadedEnemies = false;
    private bool loadingEnemies = false;
    private bool finishedLoading = false;

    public void Regenerate()
    {
        // Dont regenerate if level isnt even loaded
        if (!finishedLoading)
            return;

        foreach (var room in dungeonRooms)
            Destroy(room.gameObject);
        dungeonRooms = new List<AbstractDungeonRoom>();
        dungeonMaze = null;
        dungeonBounds = new Bounds();

        Destroy(walls.gameObject);
        Destroy(floor.gameObject);
        Destroy(objects.gameObject);
        Destroy(barriers.gameObject);
        Destroy(enemies.gameObject);

        Generate();
    }

    public void Generate()
    {
        // Create tilemaps
        walls = CreateTilemap("Walls");
        wallRenderer = walls.GetComponent<TilemapRenderer>();
        walls.gameObject.AddComponent<TilemapCollider2D>();

        floor = CreateTilemap("Floor");
        floorRenderer = floor.GetComponent<TilemapRenderer>();

        objects = CreateTilemap("Objects");
        objects.gameObject.AddComponent<TilemapCollider2D>();
        objects.gameObject.AddComponent<TilemapHoles>();
        objects.GetComponent<TilemapCollider2D>().isTrigger = true;
        objectsRenderer = objects.GetComponent<TilemapRenderer>();

        barriers = CreateTilemap("Barriers");
        barriers.gameObject.AddComponent<TilemapCollider2D>();

        // Setup tilemaps
        wallRenderer.gameObject.layer = LayerMask.NameToLayer("Obstacles");
        wallRenderer.sortingLayerName = "Obstacles";
        wallRenderer.sortingOrder = 10;

        floorRenderer.gameObject.layer = LayerMask.NameToLayer("World");
        floorRenderer.sortingLayerName = "Ground";
        floorRenderer.sortingOrder = -1;

        objectsRenderer.gameObject.layer = LayerMask.NameToLayer("SeeThroughObstacles");
        objectsRenderer.sortingLayerName = "Obstacles";
        objectsRenderer.sortingOrder = 9;

        barriers.gameObject.layer = LayerMask.NameToLayer("NoEnemyZone");

        enemies = new GameObject("Enemies");
        enemies.transform.parent = transform;

        loadedEnemies = false;
        loadedMaze = false;
        loadedPathfinding = false;
        loadedRooms = false;

        loadingEnemies = false;
        loadingMaze = false;
        loadingPathfinding = false;
        loadingRooms = false;

        finishedLoading = false;
    }

    private void Update()
    {
        if (finishedLoading)
            return;

        if (!loadingMaze)
        {
            Debug.Log(TAG + "Generating maze");
            StartCoroutine(GenerateMaze().GetEnumerator());
            loadingMaze = true;
        }

        if (!loadingRooms && loadedMaze)
        {
            StopCoroutine(GenerateMaze().GetEnumerator());
            Debug.Log(TAG + "Generating rooms");
            loadingRooms = true;
            StartCoroutine(GenerateRooms().GetEnumerator());
        }

        if (!loadingPathfinding && loadedRooms)
        {
            StopCoroutine(GenerateRooms().GetEnumerator());
            Debug.Log(TAG + "Generating enemy pathfinding");
            loadingPathfinding = true;
            StartCoroutine(UpdatePathfinding().GetEnumerator());
        }

        if (!loadingEnemies && loadedPathfinding)
        {
            StopCoroutine(UpdatePathfinding().GetEnumerator());
            Debug.Log(TAG + "Spawning enemies");
            loadingEnemies = true;
            StartCoroutine(SpawnEnemies().GetEnumerator());
        }

        if (!finishedLoading && loadedMaze && loadedRooms && loadedPathfinding && loadedEnemies)
        {
            StopCoroutine(SpawnEnemies().GetEnumerator());
            finishedLoading = true;
            FinishLoading();
        }

        UpdateProgress();
    }

    private void FinishLoading()
    {
        spawnRoom = GetComponentInChildren<DungeonSpawnRoom>();
        shopRoom = GetComponentInChildren<DungeonShopRoom>();
        bossRoom = GetComponentInChildren<DungeonBossRoom>();

        // Spawn a random starter weapon
        Instantiate(starterWeapons[GameManager.rng.Next(starterWeapons.Length)], spawnRoom.GetRandomPointInRoom(1, 1), Quaternion.identity);

        Debug.Log(TAG + "Dungeon loaded");
        GeneratedEvent.Raise();
    }

    private void UpdateProgress()
    {
        loadingProgress = (mazeGenProgress + roomGenProgress + pathfindingProgress + enemySpawnProgress) / 4;

        //Debug.Log(loadingProgress + " " + mazeGenProgress + " " + roomGenProgress + " " + pathfindingProgress + " " + enemySpawnProgress);
        // In case ui isnt loaded yet
        if (UIManager.loadingScreen != null)
            UIManager.loadingScreen.Progress = loadingProgress;
    }

    IEnumerable GenerateMaze()
    {
        float numberOfRooms = 0;
        bool canPlace = false;
        Vector2 pos = new Vector2(dungeonSize / 2, dungeonSize / 2);
        dungeonMaze = new RoomType[dungeonSize, dungeonSize];

        while (numberOfRooms < dungeonSize)
        {
            // Spawn room
            if (numberOfRooms == 0)
            {
                dungeonMaze.SetValue(RoomType.Spawn, (int)pos.x, (int)pos.y);
                numberOfRooms++;
            }

            while (!canPlace)
            {
                // Move pos
                int dir = GameManager.rng.Next(1, 5);

                switch (dir)
                {
                    case 4:
                        pos.y += 1;
                        break;
                    case 3:
                        pos.y -= 1;
                        break;
                    case 2:
                        pos.x += 1;
                        break;
                    case 1:
                        pos.x -= 1;
                        break;
                }

                // Clamp to dungeon bounds
                if (pos.x < 0)
                    pos.x += 1;
                if (pos.x >= dungeonSize)
                    pos.x -= 1;
                if (pos.y < 0)
                    pos.y += 1;
                if (pos.y >= dungeonSize)
                    pos.y -= 1;

                // Make sure we aren't on top of another room
                if (dungeonMaze[(int)pos.x, (int)pos.y] == 0)
                {
                    canPlace = true;
                }
            }

            // Shop room
            if (numberOfRooms == dungeonSize / 2)
            {
                dungeonMaze.SetValue(RoomType.Shop, (int)pos.x, (int)pos.y);
                numberOfRooms++;
                canPlace = false;
                continue;
            }

            // Boss room
            if (numberOfRooms == dungeonSize - 1)
            {
                dungeonMaze.SetValue(RoomType.Boss, (int)pos.x, (int)pos.y);
                numberOfRooms++;
                canPlace = false;
                continue;
            }

            // Normal room
            dungeonMaze.SetValue(RoomType.Normal, (int)pos.x, (int)pos.y);
            numberOfRooms++;
            canPlace = false;
            mazeGenProgress = numberOfRooms / dungeonSize;

            yield return new WaitForSeconds(0.1f);
        }

        mazeGenProgress = 1;
        loadedMaze = true;
    }

    IEnumerable GenerateRooms()
    {
        float roomCount = 0;

        for (int col = 0; col < dungeonMaze.GetLength(0); col++)
        {
            for (int row = 0; row < dungeonMaze.GetLength(1); row++)
            {
                if (dungeonMaze[col, row] != 0)
                {
                    AbstractDungeonRoom room = CreateRoom(dungeonMaze[col, row]);
                    room.dungeon = this;
                    room.Generate(col, row);

                    // Add room to rooms list and expand bounds
                    dungeonRooms.Add(room);

                    if (dungeonBounds.size.x < 1)
                        dungeonBounds = new Bounds(room.Bounds.center, room.Bounds.size);
                    else
                        dungeonBounds.Encapsulate(room.Bounds);

                    roomCount++;
                    roomGenProgress = roomCount / dungeonSize;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        // Expand bounds after rooms are all generated to provide padding
        dungeonBounds.Expand(5);

        roomGenProgress = 1;
        loadedRooms = true;
    }

    IEnumerable UpdatePathfinding()
    {
        // Wait for a few frames so (tilemap colliders are updated) before updating pathfinding
        yield return new WaitForSeconds(0.1f);
        float roomCount = 0;

        // Set dungeon bounds
        var dungeonGraph = (GridGraph)AstarPath.active.graphs[0];
        dungeonGraph.center = dungeonBounds.center;
        dungeonGraph.UpdateTransform();
        dungeonGraph.SetDimensions((int)dungeonBounds.size.x * 4, (int)dungeonBounds.size.y * 4, dungeonGraph.nodeSize);
        dungeonGraph.Scan();

        foreach (var room in dungeonRooms)
        {
            room.UpdatePathfinding();
            roomCount++;

            pathfindingProgress = roomCount / dungeonSize;

            yield return new WaitForSeconds(0.1f);
        }

        loadedPathfinding = true;
    }

    IEnumerable SpawnEnemies()
    {
        float roomCount = 0;

        foreach (var room in dungeonRooms)
        {
            room.SpawnEnemies(roomDifficulty);
            roomCount++;

            enemySpawnProgress = roomCount / dungeonSize;
            yield return new WaitForSeconds(0.1f);
        }

        loadedEnemies = true;
    }

    private Tilemap CreateTilemap(string name)
    {
        GameObject tilemapGO = new GameObject(name, typeof(Tilemap), typeof(TilemapRenderer));
        tilemapGO.transform.parent = transform;

        return tilemapGO.GetComponent<Tilemap>();
    }

    private AbstractDungeonRoom CreateRoom(RoomType type)
    {
        GameObject roomGO;

        switch (type)
        {
            case RoomType.Spawn:
                roomGO = new GameObject("Spawn Room", typeof(DungeonSpawnRoom));
                break;
            case RoomType.Shop:
                roomGO = new GameObject("Shop Room", typeof(DungeonShopRoom));
                break;
            case RoomType.Boss:
                roomGO = new GameObject("Boss Room", typeof(DungeonBossRoom));
                break;
            default:
                roomGO = new GameObject("Dungeon Room", typeof(DungeonRoom));
                break;
        }

        // Setup game object;
        roomGO.transform.parent = transform;

        return roomGO.GetComponent<AbstractDungeonRoom>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(dungeonBounds.center, dungeonBounds.size);
    }

    public enum RoomType
    {
        Empty, Normal, Spawn, Shop, Boss
    }
}
