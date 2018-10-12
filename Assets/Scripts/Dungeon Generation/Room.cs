using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour 
{
    public Exits[] exits;

    private void Awake() 
    {
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Camera.main.GetComponent<CameraController>().MoveTo(transform.position);
    }
}

public enum Exits 
{
    UP, DOWN, LEFT, RIGHT
}
