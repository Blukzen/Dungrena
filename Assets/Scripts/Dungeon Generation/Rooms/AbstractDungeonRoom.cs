using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public abstract class AbstractDungeonRoom : MonoBehaviour
{
    public DungeonGenerator dungeon;

    private int col, row; // Index in the dungeon maze.
    private Bounds bounds;
    public Bounds Bounds
    {
        get
        {
            return bounds;
        }
    }

    public virtual void Generate(int _col, int _row)
    {
        col = _col;
        row = _row;

        transform.position = new Vector2(col * (dungeon.roomWidth + dungeon.roomSpacing), row * (dungeon.roomHeight + dungeon.roomSpacing));
        bounds = new Bounds(transform.position, new Vector3(dungeon.roomWidth, dungeon.roomHeight, 1000));

        BuildFloor();
        BuildConnections();
        BuildWalls();
        SetupRoom();
    }

    /**
     * Need to scan larger than bounding space for placing walls to account for
     * room connection pathways.
     */
    protected virtual void BuildWalls()
    {
        var walls = dungeon.WallTilemap;
        var floor = dungeon.FloorTilemap;
        var tile = dungeon.wallTile;

        // Set starting point
        var xPos = bounds.min.x - dungeon.roomSpacing / 2;
        var yPos = bounds.max.y + dungeon.roomSpacing / 2;

        // Scan area place walls where appropriate
        for (float x = xPos; x < bounds.max.x + dungeon.roomSpacing / 2; x++)
        {
            for (float y = yPos; y > bounds.min.y - dungeon.roomSpacing / 2; y--)
            {
                // Top row
                if (floor.GetTile(new Vector3Int((int)x, (int)y, 0)) != null && floor.GetTile(new Vector3Int((int)x, (int)y + 1, 0)) == null)
                {
                    walls.SetTile(new Vector3Int((int)x, (int)y, 0), tile);
                    walls.SetTile(new Vector3Int((int)x, (int)y + 1, 0), tile);
                }

                // Bottom row
                if (floor.GetTile(new Vector3Int((int)x, (int)y, 0)) != null && floor.GetTile(new Vector3Int((int)x, (int)y - 1, 0)) == null)
                {
                    walls.SetTile(new Vector3Int((int)x, (int)y, 0), tile);
                    walls.SetTile(new Vector3Int((int)x, (int)y - 1, 0), tile);
                }

                // Left side
                if (floor.GetTile(new Vector3Int((int)x, (int)y, 0)) != null && floor.GetTile(new Vector3Int((int)x - 1, (int)y, 0)) == null)
                {
                    walls.SetTile(new Vector3Int((int)x, (int)y, 0), tile);
                }

                // Right side
                if (floor.GetTile(new Vector3Int((int)x, (int)y, 0)) != null && floor.GetTile(new Vector3Int((int)x + 1, (int)y, 0)) == null)
                {
                    walls.SetTile(new Vector3Int((int)x + 1, (int)y, 0), tile);
                }

                // Top right corners
                if (floor.GetTile(new Vector3Int((int)x, (int)y, 0)) != null && floor.GetTile(new Vector3Int((int)x + 1, (int)y + 1, 0)) == null ||
                    floor.GetTile(new Vector3Int((int)x, (int)y, 0)) == null && floor.GetTile(new Vector3Int((int)x + 1, (int)y + 1, 0)) != null)
                {
                    walls.SetTile(new Vector3Int((int)x + 1, (int)y + 1, 0), tile);
                }

                // Bottom right corners
                if (floor.GetTile(new Vector3Int((int)x, (int)y, 0)) != null && floor.GetTile(new Vector3Int((int)x + 1, (int)y - 1, 0)) == null ||
                    floor.GetTile(new Vector3Int((int)x, (int)y, 0)) == null && floor.GetTile(new Vector3Int((int)x + 1, (int)y - 1, 0)) != null)
                {
                    walls.SetTile(new Vector3Int((int)x + 1, (int)y - 1, 0), tile);
                }
            }
        }

    }

    protected virtual void BuildFloor()
    {
        var floor = dungeon.FloorTilemap;
        var tile = dungeon.floorTile;

        for (float x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (float y = bounds.min.y; y < bounds.max.y; y++)
            {
                floor.SetTile(new Vector3Int((int)x, (int)y, 0), tile);
            }
        }
    }

    protected virtual void SetupRoom()
    {
        var floor = dungeon.FloorTilemap;
        var objects = dungeon.ObjectTilemap;

        dungeon.roomSet.RandomizeCurrentMap();
        PlaceTiles(dungeon.roomSet.GetFloorMap(), floor);
        PlaceTiles(dungeon.roomSet.GetObjectMap(), objects);

        // Block off room exits
    }

    protected void PlaceTiles(TileBase[] tiles, Tilemap tileMap)
    {
        int index = 0;

        for (int xPos = (int)bounds.min.x; xPos < bounds.max.x; xPos++)
        {
            for (int yPos = (int)bounds.min.y; yPos < bounds.max.y; yPos++)
            {
                var currentTile = tiles[index];
                index++;

                tileMap.SetTile(new Vector3Int((int)xPos, (int)yPos, 0), currentTile);
            }
        }
    }

    /** Each room builds the connections to any neighbouring rooms.
     *  only to the room below and room to the left to avoid connecting 
     *  the same rooms multiple times.
     */
    protected virtual void BuildConnections()
    {
        var maze = dungeon.GetMaze();
        bool up = false, left = false;

        if (row + 1 < dungeon.dungeonSize)
            up = maze[col, row + 1] != DungeonGenerator.RoomType.Empty;
        if (col + 1 < dungeon.dungeonSize)
            left = maze[col + 1, row] != DungeonGenerator.RoomType.Empty;

        var floor = dungeon.FloorTilemap;
        var floorTile = dungeon.floorTile;

        var barriers = dungeon.BarrierTilemap;
        var barrierTile = dungeon.barrierTile;

        if (up)
        {
            placeTileRect(new Vector2(transform.position.x - (dungeon.roomConnectionWidth / 2), transform.position.y + (dungeon.roomHeight / 2) + (dungeon.roomSpacing - 1)), dungeon.roomConnectionWidth, dungeon.roomSpacing, floorTile, floor);
            placeTileRect(new Vector2(transform.position.x - (dungeon.roomConnectionWidth / 2), transform.position.y + (dungeon.roomHeight / 2) + (dungeon.roomSpacing - 3)), dungeon.roomConnectionWidth, dungeon.roomSpacing - 3, barrierTile, barriers);
        }

        if (left)
        {
            placeTileRect(new Vector2(transform.position.x + (dungeon.roomWidth / 2), transform.position.y + (dungeon.roomConnectionWidth / 2) - 1), dungeon.roomSpacing, dungeon.roomConnectionWidth, floorTile, floor);
            placeTileRect(new Vector2(transform.position.x + (dungeon.roomWidth / 2) + 2, transform.position.y + (dungeon.roomConnectionWidth / 2) - 1), dungeon.roomSpacing - 3, dungeon.roomConnectionWidth, barrierTile, barriers);
        }
    }

    public virtual void UpdatePathfinding()
    {
        AstarPath astar = AstarPath.active;

        // Expand bounds to include room connections;
        var fullBounds = new Bounds(bounds.center, bounds.size);
        fullBounds.Expand(dungeon.roomSpacing);
        astar.UpdateGraphs(fullBounds);
    }

    protected void placeTileRect(Vector2 topLeft, int width, int height, Tile tile, Tilemap tilemap)
    {
        for (float x = topLeft.x; x < topLeft.x + width; x++)
        {
            for (float y = topLeft.y; y > topLeft.y - height; y--)
            {
                tilemap.SetTile(new Vector3Int((int)x, (int)y, 0), tile);
            }
        }
    }

    protected void SpawnEnemies(int numEnemies)
    {
        for (int num = 0; num < numEnemies; num++)
        {
            var prefab = dungeon.spawnList.GetRandEnemy();
            if (prefab == null)
            {
                Debug.LogWarning("[Dungeon Room] recieved null enemy prefab from spawnList");
                return;
            }

            var enemy = Instantiate(prefab, GetRandomPointInRoom(5, 5), Quaternion.identity);
            enemy.transform.parent = dungeon.Enemies.transform;
        }
    }

    public Vector2 GetRandomPointInRoom(int paddingX, int paddingY)
    {
        Vector2 pos = new Vector2(Random.Range(bounds.min.x + paddingX, bounds.max.x - paddingX), Random.Range(bounds.min.y + paddingY, bounds.max.y - paddingY));

        while (true)
        {
            // Check if theres a tile there
            if (dungeon.ObjectTilemap.GetTile(new Vector3Int((int) pos.x, (int) pos.y, 0)) == null)
            {
                return pos;
            }

            pos = new Vector2(Random.Range(bounds.min.x + paddingX, bounds.max.x - paddingX), Random.Range(bounds.min.y + paddingY, bounds.max.y - paddingY));
        }
    }

    public bool IsPointInRoom(Vector2 point)
    {
        return point.x > bounds.min.x && point.x < bounds.max.x && point.y > bounds.min.y && point.y < bounds.max.y;
    }

    private void OnDrawGizmosSelected()
    {
        var newBounds = new Bounds(bounds.center, bounds.size);
        newBounds.Expand(-2);

        Gizmos.DrawWireCube(newBounds.center, newBounds.size);
    }
}
