using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : Singleton<LevelChanger> 
{
    private static bool gameReady = false;
    private Animator animator;
    private int levelToLoad;

    private void Start() 
    {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (gameReady) {
            animator.SetTrigger("FadeIn");
            gameReady = false;
        }
    }

    public void LoadNextScene() {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FadeToLevel (int levelIndex) 
    {
        animator.SetTrigger("FadeOut");
        levelToLoad = levelIndex;
    }

    public void OnFadeComplete() 
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public static void LevelReady() {
        gameReady = true;
    }
}
