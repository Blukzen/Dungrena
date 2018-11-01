using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static string TAG = "[GameManager] ";

    public static Player player;
    public Player playerPrefab;

    public static DungeonManager dungeonManager;
    public static EnemySpawner enemySpawner;

    public AstarPath astarPrefab;
    private AstarPath astarPath;

    private int currentScene;
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
        currentScene = scene.buildIndex;

        if (currentScene > 0)
        {
            var dungeonManagerOBJ = GameObject.Find(DungeonManager.TAG);

            if (dungeonManagerOBJ == null)
            {
                Debug.Log(TAG + "No dungeonManager. Dungeon will no be generated");
                SetupTestLevel();
                return;
            }

            dungeonManager = dungeonManagerOBJ.GetComponent<DungeonManager>();
            enemySpawner = GetComponent<EnemySpawner>();

            if (astarPath != null)
                Destroy(astarPath);

            astarPath = Instantiate(astarPrefab);
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

    private void SetupTestLevel()
    {
        // Check if there is already a player in the scene
        var playerOBJ = GameObject.FindGameObjectWithTag("Player");

        if (playerOBJ != null)
            player = playerOBJ.GetComponent<Player>();
    }
}
