using UnityEngine;
using System.Collections;

public class SpriteSort : MonoBehaviour
{
    public bool StaticObject;
    private SpriteRenderer sprite;
    private Collider2D col;
    private int spriteSortingOrderBase = 5000;

    private bool updatedStaticObject = false;

    // Use this for initialization
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StaticObject && updatedStaticObject)
            return;

        UpdateSprite();

        if (!updatedStaticObject) updatedStaticObject = true;
    }

    private void UpdateSprite()
    {
        sprite.sortingOrder = (int)(spriteSortingOrderBase - (col.bounds.center.y * 10));
    }
}
