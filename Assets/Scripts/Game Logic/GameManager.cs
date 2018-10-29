using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static Player player;
    public Player playerPrefab;

    public static DungeonManager dungeonManager;
    public static EnemySpawner enemySpawner;
    private Camera mainCamera;

    public int score = 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.buildIndex > 0)
        {
            dungeonManager = GameObject.Find(DungeonManager.TAG).GetComponent<DungeonManager>();
            enemySpawner = GetComponent<EnemySpawner>();

            dungeonManager.Initialize();
        }
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
        SpawnPlayer();
    }

    public void RegenerateDungeon() 
    {
        enemySpawner.KillAll();
        dungeonManager.Regenerate();
    }
}
