using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Animator))]
public abstract class MeleeWeapon : AbstractWeapon 
{
    public int secondaryAttackDamage;

    public LayerMask collisionMask; // Collision layer to look for enemies to attack.
    public float knockBackForce; // Knockback force applied to hit enemy.
    public GameObject hitEffect; // Prefab of particle effect for hitting enemy.
    public TrailRenderer swordEffect;

    [Header("Animations")]
    // Name of animation states for attacks in animator controller.
    public string mainAttackAnimation = "Default";
    public string secondaryAttackAnimation = "Default";

    protected CircleCollider2D hitCollider;

    protected int currentDamage; // This value is the damage of the current attack/damage applied on hit.

    private void Start()
    {
        hitCollider = GetComponent<CircleCollider2D>();
        //hitCollider.isTrigger = true;
        //hitCollider.enabled = false;

        if (swordEffect)
            swordEffect.enabled = false;

        currentDamage = attackDamage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) {
        // Dont do anything if the items dropped on the ground
        if (onGround)
            return;

        // if the collider the melee weapon is colliding with is not on the targeted layer mask, we do nothing.
        if ((collisionMask.value & (1 << collider.gameObject.layer)) == 0) {
            return;
        }

        // if the collider the melee weapon is colliding with is its owner (the player), we do nothing.
        var isOwner = collider.gameObject == Owner;
        if (isOwner) {
            return;
        }

        // if the collider hits a entity apply damage, knockback and spawn hit effect.
        var entity = collider.GetComponent<AbstractEntity>();
        if (entity != null) {
            entity.ApplyAttack(currentDamage, knockBackForce, Owner);
        }
    }

    // Called to do a generic attack. string animation is the name of the animation to play on attack.
    protected void MeleeAttack(string animation) 
    {
        if (!CanAttack()) return;
        animator.Play(animation);
    }

    // Enables hitCollider to start attack. Used in animation events.
    protected void AttackBegin() 
    {
        hitCollider.enabled = true;
        if (swordEffect)
            swordEffect.enabled = true;
    }

    // Disables hitCollider to end attack. Used in animation events.
    protected void AttackEnd() 
    {
        hitCollider.enabled = false;
        if (swordEffect)
            swordEffect.enabled = false;
    }

    public override void pickup(AbstractEntity entity)
    {
        base.pickup(entity);
        animator.enabled = true;
        hitCollider.enabled = false;
        transform.localScale = new Vector3(1, 1);
    }

    public override void Drop(Vector2 position)
    {
        base.Drop(position);
        animator.enabled = false;
        hitCollider.enabled = true;
        transform.localScale = new Vector3(1, 1);
    }
}
