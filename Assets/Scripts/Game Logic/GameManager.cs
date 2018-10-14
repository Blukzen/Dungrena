using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Player player;
    public Player playerPrefab;

    public static DungeonManager dungeonManager;
    public static EnemySpawner enemySpawner;
    private Camera mainCamera;

    public int score = 0;

    public void Start() {
        enemySpawner = GetComponent<EnemySpawner>();
    }

    private void SpawnPlayer() 
    {
        if (player == null)
            player = Instantiate(playerPrefab);

        var spawnRoom = dungeonManager.SpawnRoom;

        player.transform.position = spawnRoom.transform.position;

        Camera.main.GetComponent<CameraController>().MoveTo(new Vector2(spawnRoom.transform.position.x, spawnRoom.transform.position.y));
        Camera.main.transform.position = player.transform.position;

        LevelChanger.LevelReady();
    }

    public void DungeonGenerated() 
    {
        dungeonManager = GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
        SpawnPlayer();
    }

    public void RegenerateDungeon() 
    {
        enemySpawner.KillAll();
        dungeonManager.Regenerate();
    }
}
