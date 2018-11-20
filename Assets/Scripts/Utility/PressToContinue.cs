using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressToContinue : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
            GameManager.instance.LoadScene("Main Menu");
    }
}
