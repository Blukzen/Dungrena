using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapHoles : MonoBehaviour
{
    private TilemapCollider2D tilemapCollider;

    private void Start()
    {
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var entity = collision.gameObject.GetComponent<AbstractEntity>();

        if (entity == null)
        {
            return;
        }

        var entityBounds = collision.bounds;

        // Check bottom edge
        if (!tilemapCollider.OverlapPoint(new Vector2(entityBounds.min.x, entityBounds.min.y)))
            return;
        if (!tilemapCollider.OverlapPoint(new Vector2(entityBounds.max.x, entityBounds.min.y)))
            return;

        // Check top edge
        if (!tilemapCollider.OverlapPoint(new Vector2(entityBounds.min.x, entityBounds.max.y)))
            return;
        if (!tilemapCollider.OverlapPoint(new Vector2(entityBounds.max.x, entityBounds.max.y)))
            return;

        entity.Fall();
    }
}
