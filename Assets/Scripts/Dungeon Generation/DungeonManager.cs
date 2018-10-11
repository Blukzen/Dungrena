using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour 
{
    private int numRooms = 10;
    private int dungeonSize = 10;

    private int[,] maze;

    public GameObject prefab;

    // Important room locations.
    private Vector2 startRoom;
    private Vector2 shopRoom;
    private Vector2 bossRoom;

    private void Start() 
    {
        GenerateMaze();
        BuildDungeon();
    }

    void GenerateMaze() 
    {
        maze = new int[dungeonSize * 2, dungeonSize * 2];

        System.Random rand = new System.Random();

        Vector2 pos = new Vector2(dungeonSize, dungeonSize);

        for (int i = 0; i <= numRooms; i++) 
        {
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

            // Check to stay within dungeon bounds
            if (pos.x < 0)
                pos.x += 1;
            if (pos.x > dungeonSize * 2 - 1)
                pos.x -= 1;
            if (pos.y < 0)
                pos.y += 1;
            if (pos.y > dungeonSize * 2 - 1)
                pos.y -= 1;

            Debug.Log("Position: " + pos.x + ", " + pos.y);
            maze.SetValue(1, (int) pos.x, (int) pos.y);

            if (i == 0)
                startRoom = new Vector2(pos.x , pos.y);
            if (i == numRooms / 2)
                shopRoom = new Vector2(pos.x, pos.y);
            if (i == numRooms)
                bossRoom = new Vector2(pos.x, pos.y);
        }
    }

    void BuildDungeon() {
        for (int col = 0; col < maze.GetLength(0); col++) 
        {
            for (int row = 0; row < maze.GetLength(1); row++) 
            {
                if (maze[col, row].Equals(1)) {
                    var obj = Instantiate(prefab);
                    obj.transform.position = new Vector2(col, row);

                    if (col == startRoom.x && row == startRoom.y)
                        obj.GetComponent<SpriteRenderer>().color = Color.green;
                    if (col == shopRoom.x && row == shopRoom.y)
                        obj.GetComponent<SpriteRenderer>().color = Color.yellow;
                    if (col == bossRoom.x && row == bossRoom.y)
                        obj.GetComponent<SpriteRenderer>().color = Color.red;

                }
            }
        }
    }
}
