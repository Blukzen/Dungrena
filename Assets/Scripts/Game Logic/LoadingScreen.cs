using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public StatsBar loadingBar;

    private bool isLoading;
    private float progress;
    public float Progress { set { progress = value; } }

    private Animator animator;

    public void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isLoading)
        {
            loadingBar.UpdateBar(progress, 1);
        } if (progress == 1)
        {
            animator.SetTrigger("isLoading");
        }
    }

    public void BeginLoading()
    {
        isLoading = true;
        GameManager.StartLoading();
    }

    public void EndLoading()
    {
        isLoading = false;
        loadingBar.UpdateBar(0, 1);
        gameObject.SetActive(false);
    }
}
