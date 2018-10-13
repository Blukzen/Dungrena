using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour 
{
    public Exits[] exits;
    public RoomType roomType = RoomType.NORMAL;
    public AbstractEntity[] entities;
    public int difficulty = 0;

    private BoxCollider2D roomArea;

    private void Start() 
    {
        roomArea = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Camera.main.GetComponent<CameraController>().MoveTo(transform.position);
    }

    public void Init() {
        Debug.Log("Initializing room");
        switch(roomType) {
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

    private void InitNormalRoom() {
        SpawnEnemies();
    }

    private void InitSpawnRoom() {

    }

    private void InitShopRoom() {

    }

    private void InitBossRoom() {

    }

    private void SpawnEnemies() {
        var entity = Instantiate(entities[0]);
        entity.transform.position = RandomPointInBox(roomArea.transform.position, roomArea.size);
    }

    private void SpawnTraps() {

    }

    private static Vector2 RandomPointInBox(Vector2 center, Vector2 size) {
        return center + new Vector2(
           (Random.value - 0.5f) * size.x,
           (Random.value - 0.5f) * size.y
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
