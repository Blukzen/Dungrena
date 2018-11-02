using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public static string TAG = "[DungeonGenerator] ";

    public int roomHeight;
    public int roomWidth;
    public int roomSpacing;
    public int roomConnectionWidth;
    public int dungeonSize;

    public RuleTile wallTile;
    public WeightedRandomTile floorTile;

    private Bounds dungeonBounds;
    private List<AbstractDungeonRoom> dungeonRooms = new List<AbstractDungeonRoom>();
    private RoomType[,] dungeonMaze;
    public RoomType[,] GetMaze()
    {
        return dungeonMaze;
    }

    private Tilemap walls;
    public Tilemap WallTilemap { get { return walls; } }
    private Tilemap floor;
    public Tilemap FloorTilemap { get { return floor; } }
    private TilemapRenderer wallRenderer;
    private TilemapRenderer floorRenderer;

    private Coroutine generatingMaze;

    public void Start()
    {
        Generate();
    }

    public void Regenerate()
    {
        foreach (var room in dungeonRooms)
            Destroy(room.gameObject);
        dungeonRooms = new List<AbstractDungeonRoom>();

        dungeonMaze = null;

        Destroy(walls.gameObject);
        Destroy(floor.gameObject);

        Generate();
    }

    public void Generate()
    {
        // Create tilemaps
        walls = CreateTilemap("Walls");
        wallRenderer = walls.GetComponent<TilemapRenderer>();
        floor = CreateTilemap("Floor");
        floorRenderer = floor.GetComponent<TilemapRenderer>();

        // Setup tilemaps
        wallRenderer.sortingLayerName = "Obstacles";
        wallRenderer.sortingOrder = 10;

        floorRenderer.sortingLayerName = "World";

        // Generate dungeon maze
        Debug.Log(TAG + "Generating maze..");
        foreach(float progress in GenerateMaze())
        {
            Debug.Log(TAG + "Generating maze.. " + progress * 100 + "%");
        }

        Debug.Log(TAG + "Maze generated");

        // Create rooms
        Debug.Log(TAG + "Generating rooms..");
        foreach(float progress in GenerateRooms())
        {
            Debug.Log(TAG + "Generating rooms.. " + progress * 100 + "%");
        }
    }

    IEnumerable<float> GenerateMaze()
    {
        float numberOfRooms = 0;
        bool canPlace = false;
        Vector2 pos = new Vector2(dungeonSize / 2, dungeonSize / 2);
        dungeonMaze = new RoomType[dungeonSize, dungeonSize];

        System.Random rand = new System.Random((int)(Time.time * 1000));

        while (numberOfRooms < dungeonSize)
        {
            // Spawn room
            if (numberOfRooms == 0)
            {
                dungeonMaze.SetValue(RoomType.Spawn, (int) pos.x, (int) pos.y);
                numberOfRooms++;
            }

            while (!canPlace)
            {
                // Move pos
                int dir = rand.Next(1, 4);

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
                if (dungeonMaze[(int) pos.x, (int) pos.y] == 0)
                {
                    canPlace = true;
                }
            }

            // Shop room
            if (numberOfRooms == dungeonSize/2)
            {
                dungeonMaze.SetValue(RoomType.Shop, (int)pos.x, (int)pos.y);
                numberOfRooms++;
                canPlace = false;
                continue;
            }

            // Boss room
            if (numberOfRooms == dungeonSize-1)
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

            yield return numberOfRooms/dungeonSize;
            if (numberOfRooms == dungeonSize) yield return 1;
        }
    }

    IEnumerable<float> GenerateRooms()
    {
        float roomsGenerated = 0;

        for (int col = 0; col < dungeonMaze.GetLength(0); col++)
        {
            for (int row = 0; row < dungeonMaze.GetLength(1); row++)
            {
                if (dungeonMaze[col, row] != 0)
                {
                    AbstractDungeonRoom room = CreateRoom(dungeonMaze[col, row]);
                    room.dungeon = this;
                    room.Generate(col, row);

                    dungeonRooms.Add(room);
                    roomsGenerated++;
                    yield return roomsGenerated/dungeonSize;
                }
            }
        }
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

        switch(type)
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

    public enum RoomType
    {
        Empty, Normal, Spawn, Shop, Boss
    }
}
