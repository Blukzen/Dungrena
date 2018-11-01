using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D), typeof(Animator))]
public abstract class AbstractItem : MonoBehaviour
{
    public string ItemName;
    public string ItemDescription;
    public string ItemSlug;
    [HideInInspector]
    public ItemType ItemType;

    protected PolygonCollider2D itemCollider;
    protected Animator animator;
    protected new SpriteRenderer renderer;

    protected AbstractEntity Owner;
    protected bool OnGround = true;

    private void Awake()
    {
        itemCollider = GetComponent<PolygonCollider2D>();
        itemCollider.isTrigger = false;
        itemCollider.enabled = true;

        renderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public virtual AbstractItem Pickup(AbstractEntity entity)
    {
        Owner = entity;
        OnGround = false;

        gameObject.layer = LayerMask.NameToLayer("Player");

        itemCollider.enabled = false;
        itemCollider.isTrigger = true;
        animator.enabled = true;

        return this;
    }

    public virtual void Drop()
    {
        Owner = null;
        OnGround = true;

        gameObject.layer = LayerMask.NameToLayer("Items");

        itemCollider.isTrigger = false;
        itemCollider.enabled = true;

        animator.enabled = false;

        renderer.sortingOrder = 0;

        transform.Rotate(0, 0, Random.Range(0, 360));

        transform.parent = null;
    }

    public virtual void MouseOver()
    {

    }

    public virtual void MouseExit()
    {

    }
}

public enum ItemType
{
    Weapon
}
