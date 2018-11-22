using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public static int score = 0;

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

    public void LoadScene(string sceneName)
    {
        UIManager.loadingScreen.gameObject.SetActive(true);
        sceneToLoad = SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    public void ReturnToMenu()
    {
        UIManager.BackToMenu();
        Destroy(astarPath.gameObject);
        score = 0;
        UIManager.ResetHUD();
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        UIManager.HideHUD();
        UIManager.GameOver();
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
        }
    }

    private void SpawnPlayer()
    {
        if (player == null)
            player = Instantiate(playerPrefab);

        Camera.main.transform.position = dungeonGenerator.SpawnRoom.transform.position;
        player.transform.position = dungeonGenerator.SpawnRoom.GetRandomPointInRoom(3, 3);
    }

    private void SpawnEnemies()
    {

    }

    public void DungeonGenerated()
    {
        SpawnEnemies();
        SpawnPlayer();
        UIManager.ShowHUD();
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
