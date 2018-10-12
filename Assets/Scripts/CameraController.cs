using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    public void Init() 
    {
        var dungeonManager = GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
        var spawnRoom = dungeonManager.SpawnRoom;

        transform.position = new Vector3(spawnRoom.transform.position.x, spawnRoom.transform.position.y, -10);
    }
}
