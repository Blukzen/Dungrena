using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    [Range(0, 10)]
    public float damping;
    private Vector2 targetPosition;

    private void Update()
    {
        if (GameManager.player == null)
            return;

        // Test level case
        if (GameManager.player.GetCurrentRoom() == null)
            return;
        
        targetPosition = GameManager.player.GetCurrentRoom().transform.position;
        transform.position = Vector3.Lerp(this.transform.position, new Vector3(targetPosition.x, targetPosition.y, -10), damping);
    }

    public void MoveTo(Vector2 newPosition) 
    {
        targetPosition = newPosition;
    }
}