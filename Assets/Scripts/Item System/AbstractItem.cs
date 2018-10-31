using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D), typeof(Animator))]
public abstract class AbstractItem : MonoBehaviour
{
    public string ItemName;
    public string ItemDescription;
    public string ItemSlug;
    public ItemType ItemType;

    protected PolygonCollider2D itemCollider;
    protected Animator animator;

    protected AbstractEntity Owner;
    protected bool OnGround = true;

    private void Awake()
    {
        itemCollider = GetComponent<PolygonCollider2D>();
        itemCollider.isTrigger = false;
        itemCollider.enabled = true;

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

        transform.parent = null;
    }
}

public enum ItemType
{
    Weapon
}
