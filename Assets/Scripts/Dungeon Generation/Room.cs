using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    private BoxCollider2D spawnArea;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player")
            Camera.main.GetComponent<CameraController>().MoveTo(transform.position);
    }

    public void Init() 
    {
        spawnArea = GetComponent<BoxCollider2D>();

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

        // Set layer to world so enemys cant see into this room
        gameObject.transform.Find("Floor").gameObject.layer = LayerMask.NameToLayer("World");

    }

    private void InitShopRoom() 
    {
        Debug.Log("Initializing Shop Room");

        // Set layer to world to enemys cant see into this room
        gameObject.transform.Find("Floor").gameObject.layer = LayerMask.NameToLayer("World");

    }

    private void InitBossRoom() 
    {
        Debug.Log("Initializing Boss Room");

    }

    public Vector2 RandomPointInRoom() {
        return (Vector2) spawnArea.transform.position + new Vector2(
           (Random.value - 0.5f) * spawnArea.bounds.size.x,
           (Random.value - 0.5f) * spawnArea.bounds.size.y
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
