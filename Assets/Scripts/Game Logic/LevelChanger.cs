using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : Singleton<LevelChanger> 
{
    private Animator animator;
    private int levelToLoad;

    private void Start() 
    {
        animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
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

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
        animator.SetTrigger("FadeIn");
    }
}
