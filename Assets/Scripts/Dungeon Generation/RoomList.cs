using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RoomList", menuName = "DungeonGen/RoomList", order = 1)]
public class RoomList : ScriptableObject 
{
    public Room defaultRoom;
    public Room[] rooms;

    public Room GetRoom(Exits[] exits) 
    {
        // Search list for suitable room
        foreach (var room in rooms) 
        {
            if (room.exits.Length == exits.Length) 
            {
                for (int i = 0; i < exits.Length; i++) 
                {
                    if (room.exits[i] != exits[i]) break;
                    if (i == exits.Length - 1) return room;
                }
            }
        }

        // Return default room if suitable room not found
        return defaultRoom;
    }
}
