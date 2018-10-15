using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour 
{
    public Exits[] exits;
    public int difficulty = 0;

    private RoomType roomType = RoomType.NORMAL;
    public RoomType Type 
    {
        get { return roomType; }
        set { roomType = value; }
    }

    private BoxCollider2D roomArea;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.GetComponent<Player>() != null)
            Camera.main.GetComponent<CameraController>().MoveTo(transform.position);
    }

    public void Init() 
    {
        roomArea = GetComponent<BoxCollider2D>();

        switch (roomType) 
        {
            case RoomType.NORMAL:
                InitNormalRoom();
                break;
            case RoomType.SPAWN:
                InitSpawnRoom();
                break;
            case RoomType.SHOP:
                InitShopRoom();
                break;
            case RoomType.BOSS:
                InitBossRoom();
                break;
        }
    }

    private void InitNormalRoom() 
    {
        if (GameManager.enemySpawner != null)
            GameManager.enemySpawner.Spawn(this);
    }

    private void InitSpawnRoom() 
    {
        Debug.Log("Initializing Spawn Room");

    }

    private void InitShopRoom() 
    {
        Debug.Log("Initializing Shop Room");

    }

    private void InitBossRoom() 
    {
        Debug.Log("Initializing Boss Room");

    }

    public Vector2 RandomPointInRoom() {
        return (Vector2) roomArea.transform.position + new Vector2(
           (Random.value - 0.5f) * roomArea.size.x,
           (Random.value - 0.5f) * roomArea.size.y
        );
    }
}

public enum Exits 
{
    UP, DOWN, LEFT, RIGHT
}

public enum RoomType 
{
    SPAWN, SHOP, BOSS, NORMAL
}
