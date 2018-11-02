using Pathfinding;
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
    private bool updatedPathfinding = false;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player")
            Camera.main.GetComponent<CameraController>().MoveTo(transform.position);
    }

    private void LateUpdate()
    {
        if (!updatedPathfinding)
        {
            // Update astar graph
            var bounds = transform.Find("Floor").GetComponent<Collider2D>().bounds;
            bounds.Expand(Vector3.forward * 1000);

            var guo = new GraphUpdateObject(bounds);
            guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo);

            updatedPathfinding = true;
        }
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
        var wall = gameObject.transform.Find("Walls");
        wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");
    }

    private void InitSpawnRoom() 
    {
        name = "Spawn Room";
        Debug.Log("[" + name + "]" + " Initializing Spawn Room");

        // Set layer to NoEnemyZone so enemys cant see into this room
        var floor = gameObject.transform.Find("Floor");
        floor.gameObject.layer = LayerMask.NameToLayer("NoEnemyZone");
        floor.gameObject.GetComponent<TilemapCollider2D>().isTrigger = false;

        var wall = gameObject.transform.Find("Walls");
        wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");

    }

    private void InitShopRoom() 
    {
        name = "Shop Room";
        Debug.Log("[" + name + "]" + " Initializing Shop Room");

        // Set layer to NoEnemyZone so enemys cant see into this room
        var floor = gameObject.transform.Find("Floor");
        floor.gameObject.layer = LayerMask.NameToLayer("NoEnemyZone");
        floor.gameObject.GetComponent<TilemapCollider2D>().isTrigger = false;

        var wall = gameObject.transform.Find("Walls");
        wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");

    }

    private void InitBossRoom() 
    {
        name = "Boss Room";
        Debug.Log("[" + name + "]" + " Initializing Boss Room");

        var wall = gameObject.transform.Find("Walls");
        wall.gameObject.layer = LayerMask.NameToLayer("Obstacles");
    }

    public Vector2 RandomPointInRoom() {
        return (Vector2) spawnArea.transform.position + new Vector2(
           (Random.value - 0.5f) * (spawnArea.bounds.size.x*0.9f),
           (Random.value - 0.5f) * (spawnArea.bounds.size.y*0.9f)
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
