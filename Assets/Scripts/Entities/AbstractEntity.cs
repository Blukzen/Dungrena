using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractEntity : MonoBehaviour, IDamageable
{
    public int maxSpeed = 7;
    public float acceleration = 1;
    public float friction = 1.9f;

    protected Vector2 moveDirection;

    public float maxHealth = 10;
    public float health;

    private float pickUpRange = 2f;

    public bool canMove = true;
    public bool applyFriction = true;
    public bool attacking = false;

    protected Rigidbody2D rb2d;
    protected Collider2D col;
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected GameEvent onCollision;

    private int spriteSortingOrderBase = 5000;

    public GameObject DamageEffect;

    public virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public virtual void UpdatePhysics()
    {

        // TODO: friction and acceleration multipliers
        var _friction = friction;

        Vector2 newVel = rb2d.velocity;

        if (canMove) { newVel = UpdateMovement(newVel); }

        if (applyFriction)
        {
            // Friction when not moving
            if (moveDirection.x == 0)
                newVel.x = Approach(newVel.x, 0, _friction / 2);
            if (moveDirection.y == 0)
                newVel.y = Approach(newVel.y, 0, _friction / 2);
        }

        rb2d.velocity = newVel; 
    }
    
    protected virtual void LateUpdate()
    {
        if (canMove)
            UpdateSprite();
    }

    protected virtual void UpdateSprite()
    {
        sprite.sortingOrder = (int)(spriteSortingOrderBase - (col.bounds.center.y * 10));
    }

    public virtual Vector2 UpdateMovement(Vector2 newVel)
    {
        // Left
        if (moveDirection.x < 0)
        {
            // Apply acceleration left
            if (newVel.x > 0)
                newVel.x = Approach(newVel.x, 0, friction);
            newVel.x = Approach(newVel.x, moveDirection.x * maxSpeed, acceleration);
        }

        // Right
        if (moveDirection.x > 0)
        {
            // Apply acceleration right
            if (newVel.x < 0)
                newVel.x = Approach(newVel.x, 0, friction);
            newVel.x = Approach(newVel.x, moveDirection.x * maxSpeed, acceleration);
        }

        // Down
        if (moveDirection.y < 0)
        {
            // Apply acceleration down
            if (newVel.y > 0)
                newVel.y = Approach(newVel.y, 0, friction);
            newVel.y = Approach(newVel.y, moveDirection.y * maxSpeed, acceleration);
        }

        // Up
        if (moveDirection.y > 0)
        {
            // Apply acceleration up
            if (newVel.y < 0)
                newVel.y = Approach(newVel.y, 0, friction);
            newVel.y = Approach(newVel.y, moveDirection.y * maxSpeed, friction);
        }

        return newVel;
    }

    // TODO: Handle death event.
    public void Damage(float amount)
    {
        health -= amount;

        if (health <= 0)
            Killed();

    }

    public virtual void Fall()
    {
        if (GetComponent<AbstractEnemyAI>() != null)
            GetComponent<AbstractEnemyAI>().enabled = false;

        canMove = false;
        ResetVelocity();

        // TODO: Shrink scale here because animator scale overrides look to mouse in game.
        StartCoroutine(FallingAnim().GetEnumerator());
    }

    private IEnumerable FallingAnim() {
        float yShrinkRate = 0.009f; 
        float xShrinkRate = 0.009f;
        float timeCount = 0;

        // In case flipped
        xShrinkRate *= transform.localScale.x;

        while (transform.localScale.y > 0) {
            transform.localScale = new Vector3(transform.localScale.x - xShrinkRate, transform.localScale.y - yShrinkRate);
            transform.position = new Vector2(transform.position.x, transform.position.y - yShrinkRate);

            yield return new WaitForSeconds(0.1f);
            timeCount += Time.deltaTime;

            if (timeCount > 0.2) {
                sprite.sortingLayerName = "Default";
                sprite.sortingOrder = -10;
            }
        }

        FinishedFall();
    } 

    public virtual void FinishedFall()
    {
        Destroy(gameObject);
    }

    public virtual void ApplyAttack(float damage, float knockback, AbstractEntity attacker)
    {
        if (attacker == null)
            return;

        // Knockback
        var knockbackDirection = transform.position - attacker.transform.position;
        AddVelocity(knockbackDirection.normalized, knockback);
        DamageEffectPlay();

        // Damage
        Damage(damage);
    }

    public virtual void DamageEffectPlay()
    {
        if (DamageEffect != null) { }
            Instantiate(DamageEffect, transform.position, Quaternion.identity);
    }

    public virtual void Killed()
    {

    }

    // Set movement input
    public void SetMovement(Vector2 direction)
    {
        moveDirection = direction;
    }

    // Reset movement input
    public void StopMoving()
    {
        moveDirection = Vector2.zero;
    }

    // For adding extra velocity without moveSpeed bounds to rb2d
    public void AddVelocity(Vector2 direction, float speed)
    {
        rb2d.velocity += (direction.normalized * speed);
    }

    // For adding force to rb2d
    public void AddForce(Vector2 direction, float strength) {
        rb2d.AddForce(direction.normalized * strength);
    }

    // Reset velocity
    public void ResetVelocity()
    {
        rb2d.velocity = Vector2.zero;
    }

    // Approach end from start at an increment of shift
    public float Approach(float start, float end, float shift)
    {
        float val = 0;

        if (start < end)
        {
            val = Mathf.Min(start + shift, end);
        }
        else
        {
            val = Mathf.Max(start - shift, end);
        }

        return val;
    }

    // Pick up 
    public void PickupItem()
    {
        // Search for nearby items
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, 10, 1 << LayerMask.NameToLayer("Items"));

        // No items
        if (items.Length == 0)
            return;

        AbstractItem closestItem = null;
        float closestDistance = -1;

        // Find eligible item
        foreach (var item in items)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);

            if (distance > pickUpRange)
                continue;

            if (closestDistance == -1 || closestDistance < distance)
            {
                closestDistance = distance;
                closestItem = item.GetComponent<AbstractItem>();
            }
        }

        if (closestItem == null)
            return;

        if (closestItem.ItemType == ItemType.Weapon)
            EquipItem((AbstractWeapon)closestItem);
    }

    public abstract void EquipItem(AbstractWeapon weapon);
    public virtual void OnAttackBegin(float manaCost) {}
    public virtual void OnAttackEnd() {}
    public abstract bool CanSecondaryAttack();
}
