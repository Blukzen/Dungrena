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

    private int currentScene;
    private static int sceneToLoad;

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

    public static void StartLoading()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadNextScene()
    {
        UIManager.loadingScreen.gameObject.SetActive(true);
        sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        currentScene = scene.buildIndex;

        if (instance != null)
            return;

        if (currentScene > 0)
        {
            if (astarPath != null)
                Destroy(astarPath);

            astarPath = Instantiate(astarPrefab);

            dungeonGenerator = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>();
            dungeonGenerator.Generate();
        }
    }

    private void SpawnPlayer() 
    {
        if (player == null)
            player = Instantiate(playerPrefab);

        LevelChanger.LevelReady();
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
