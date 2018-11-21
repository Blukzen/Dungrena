using UnityEngine;

public class LootChest : MonoBehaviour
{
    public GameObject spawnEffect;
    public float openRange;

    private bool mouseOver;

    public void Open()
    {
        GetComponent<Animator>().Play("Open");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        var tilePos = transform.TransformPoint(transform.position);
        GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>().ObjectTilemap.SetTile(new Vector3Int((int)tilePos.x, (int)tilePos.y, (int)tilePos.z), null);

        Debug.Log(tilePos.ToString());
        Destroy(gameObject);
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