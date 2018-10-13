using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {

    private static string TAG = "[Dungeon Manager] ";

    public RoomList roomList;

    public int roomHeight = 10;
    public int roomWidth = 18;

    public GameEvent dungeonGeneratedEvent;

    private int numRooms = 15;
    private int dungeonSize = 10;

    private int[,] maze;
    private Room[,] dungeon;

    // Important room locations.
    private Vector2 spawnRoom;
    public Room SpawnRoom { get { return GetRoom((int)spawnRoom.x, (int)spawnRoom.y); } }

    private Vector2 shopRoom;
    public Room ShopRoom { get { return GetRoom((int)shopRoom.x, (int)shopRoom.y); } }

    private Vector2 bossRoom;
    public Room BossRoom { get { return GetRoom((int)bossRoom.x, (int)bossRoom.y); } }

    private void Start() 
    {
        Initialize();
    }

    private void Initialize() 
    {
        Debug.Log(TAG + "Generating Maze...");
        GenerateMaze();
        Debug.Log(TAG + "Building Dunegon...");
        BuildDungeon();
        if (dungeonGeneratedEvent != null)
            dungeonGeneratedEvent.Raise();
        Debug.Log(TAG + "Dungeon Complete!");
    }

    void GenerateMaze() 
    {
        maze = new int[dungeonSize * 2, dungeonSize * 2];
        dungeon = new Room[dungeonSize * 2, dungeonSize * 2];

        System.Random rand = new System.Random();

        Vector2 pos = new Vector2(dungeonSize, dungeonSize);

        for (int i = 0; i <= numRooms; i++) 
        {
            // Pick random direction to move in.
            int dir = rand.Next(1, 4);

            switch(dir) 
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

            // Check to stay within dungeon bounds.
            if (pos.x < 0)
                pos.x += 1;
            if (pos.x > dungeonSize * 2 - 1)
                pos.x -= 1;
            if (pos.y < 0)
                pos.y += 1;
            if (pos.y > dungeonSize * 2 - 1)
                pos.y -= 1;

            // Mark room location in maze array.
            maze.SetValue(1, (int) pos.x, (int) pos.y);

            // Keep reference to special rooms.
            if (i == 0)
                spawnRoom = new Vector2(pos.x , pos.y);
            if (i == numRooms / 2)
                shopRoom = new Vector2(pos.x, pos.y);
            if (i == numRooms)
                bossRoom = new Vector2(pos.x, pos.y);
        }
    }

    // Builds the dungeon from the generated maze.
    void BuildDungeon() 
    {
        for (int col = 0; col < maze.GetLength(0); col++) 
        {
            for (int row = 0; row < maze.GetLength(1); row++) 
            {
                // If co-ord has been marked as a room location
                if (maze[col, row].Equals(1)) {
                    var roomPrefab = GetRoomPrefab(col, row);
                    var room = Instantiate(roomPrefab);

                    room.transform.position = new Vector2(col * roomWidth, row * roomHeight);
                    room.transform.SetParent(transform);

                    dungeon.SetValue(room, col, row);
                }
            }
        }

        SpawnRoom.roomType = RoomType.SPAWN;
        ShopRoom.roomType = RoomType.SHOP;
        BossRoom.roomType = RoomType.BOSS;

        foreach (var room in dungeon) 
        {
            if (room != null)
                room.Init();
        }
    }

    // Picks and returns a roomtype that will fit the position x, y.
    private Room GetRoomPrefab(int x, int y) 
    {
        var exits = new ArrayList();

        // Check which exits are needed.
        // Up
        if (y + 1 < (dungeonSize * 2) && maze.GetValue(x, y + 1).Equals(1))
            exits.Add(Exits.UP);
        // Left
        if (x - 1 > 0 && maze.GetValue(x - 1, y).Equals(1))
            exits.Add(Exits.LEFT);
        // Down
        if (y - 1 > 0 && maze.GetValue(x, y - 1).Equals(1))
            exits.Add(Exits.DOWN);
        // Right
        if (x + 1 < (dungeonSize * 2) && maze.GetValue(x + 1, y).Equals(1))
            exits.Add(Exits.RIGHT);

        // Hand over to room list to find room that meets the requirements.
        return roomList.GetRoom(exits.ToArray(typeof(Exits)) as Exits[]);
    }

    public Room GetRoom(int x, int y) 
    {
        return dungeon[x, y];
    }

    public void Regenerate() 
    {
        // Destroy old dungeon
        foreach(Transform child in transform) 
        {
            Destroy(child.gameObject);
        }

        Initialize();
    }
}
