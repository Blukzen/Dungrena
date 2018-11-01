using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class MeleeWeapon : AbstractWeapon 
{
    public LayerMask collisionMask; // Collision layer to look for enemies to attack.
    public GameObject hitEffect; // Prefab of particle effect for hitting enemy.
    public TrailRenderer swordEffect;

    [Header("Animations")]
    // Name of animation states for attacks in animator controller.
    public string mainAttackAnimation = "Default";
    public string secondaryAttackAnimation = "Default";

    protected override void Start()
    {
        base.Start();

        if (swordEffect)
            swordEffect.enabled = false;

        currentDamage = attackDamage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider) {
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

    // Enables hitCollider to start attack. Used in animation events.
    protected void AttackBegin() 
    {
        itemCollider.enabled = true;
        attacking = true;

        if (swordEffect)
            swordEffect.enabled = true;

        Owner.OnAttackBegin(currentManaCost);
    }

    // Disables hitCollider to end attack. Used in animation events.
    protected void AttackEnd() 
    {
        itemCollider.enabled = false;
        attacking = false;

        if (swordEffect)
            swordEffect.enabled = false;

        lastAttackTime = Time.time;

        Owner.OnAttackEnd();
    }
}
