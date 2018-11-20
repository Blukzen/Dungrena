using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour
{
    public Text ScoreText;
    private Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        ScoreText.text = "SCORE: " + GameManager.score.ToString();
    }

    public void FadeOut()
    {
        animator.Play("GameFinished");
    }

    public void Finished()
    {
        gameObject.SetActive(false);
    }
}
