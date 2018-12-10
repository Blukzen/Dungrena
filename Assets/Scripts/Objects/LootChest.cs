using UnityEngine;
using UnityEngine.Tilemaps;

public class LootChest : MonoBehaviour
{
    public GameObject spawnEffect;
    public Tile tile;
    public float openRange;

    private bool mouseOver;
    private Tilemap tilemap;

    public void Open()
    {
        GetComponent<Animator>().Play("Open");
        tilemap = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>().ObjectTilemap;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && mouseOver)
        {
            if (Vector2.Distance(transform.position, GameManager.player.transform.position) <= openRange)
                Open();
        }
    }

    public void DropItem()
    {
        if (spawnEffect != null)
            Instantiate(spawnEffect, transform.position, Quaternion.identity);

        Instantiate(ItemDatabase.instance.items.RandomIndex(), transform.position, Quaternion.identity);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void OnMouseEnter()
    {
        Debug.Log("KkK Open The Chest");
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}