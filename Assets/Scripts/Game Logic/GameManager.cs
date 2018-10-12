using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Player player;
    public Player playerPrefab;

    private DungeonManager dungeonManager;
    private Camera mainCamera;

    public int score = 0;

    private void SpawnPlayer() 
    {
        if (player == null)
            player = Instantiate(playerPrefab);

        var spawnRoom = dungeonManager.SpawnRoom;

        player.transform.position = spawnRoom.transform.position;

        Camera.main.GetComponent<CameraController>().MoveTo(new Vector2(player.transform.position.x, player.transform.position.y));
    }

    public void DungeonGenerated() 
    {
        dungeonManager = GameObject.Find("DungeonManager").GetComponent<DungeonManager>();
        SpawnPlayer();
    }
}
