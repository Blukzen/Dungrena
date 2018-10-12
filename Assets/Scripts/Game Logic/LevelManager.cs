using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public DungeonManager dungeonManager;

    public void SpawnPlayer() {
        var spawnPoint = dungeonManager.SpawnRoom;


    }
}
