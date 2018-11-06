using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static string TAG = "[GameManager] ";

    public static Player player;
    public Player playerPrefab;

    public AstarPath astarPrefab;
    public static AstarPath astarPath;

    public static DungeonGenerator dungeonGenerator;
    public static System.Random rng;

    private int currentScene;
    private static int sceneToLoad;

    private Camera mainCamera;

    public int score = 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        rng = new System.Random(System.DateTime.Now.Millisecond);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void StartLoading()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadNextScene()
    {
        UIManager.loadingScreen.gameObject.SetActive(true);
        sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log(TAG + "Scene loaded " + scene.name);

        if (scene.name != "Main Menu")
        {
            if (astarPath != null)
                Destroy(astarPath);

            astarPath = Instantiate(astarPrefab, transform);

            var dungeonGeneratorGO = GameObject.FindGameObjectWithTag("DungeonGenerator");
            if (dungeonGeneratorGO == null)
            {
                Debug.Log(TAG + "Setting up test level");
                SetupTestLevel();
                return;
            }

            dungeonGenerator = dungeonGeneratorGO.GetComponent<DungeonGenerator>();
            dungeonGenerator.Generate();
            SpawnEnemies();
            SpawnPlayer();
            UIManager.ShowHUD();
        }
    }

    private void SpawnPlayer() 
    {
        if (player == null)
            player = Instantiate(playerPrefab);

        Camera.main.transform.position = dungeonGenerator.SpawnRoom.transform.position;
        player.transform.parent = transform;
        player.transform.position = dungeonGenerator.SpawnRoom.GetRandomPointInRoom(3, 3);
    }

    private void SpawnEnemies()
    {

    }

    public void DungeonGenerated() 
    {
        SpawnPlayer();
    }

    public void RegenerateDungeon() 
    {
    }

    private void SetupTestLevel()
    {
        // Check if there is already a player in the scene
        var playerOBJ = GameObject.FindGameObjectWithTag("Player");

        if (playerOBJ != null)
            player = playerOBJ.GetComponent<Player>();
    }
}
