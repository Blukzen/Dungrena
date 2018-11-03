using UnityEngine;
using System.Collections;

public class DashAttackState : AbstractEnemyAbilityState
{

    public float DashDamage = 2;
    public float DashKnockBack = 10;
    public float DashSpeed = 25;
    public float DashDistance = 4;

    public GameObject DashEffect;

    private Vector2 direction;
    private Vector2 startPos;

    private bool dashing = false;
    [HideInInspector]
    public bool chargingDash = true;

    public override string Name { get { return "DashAttackState"; } }

    public override void init()
    {
        base.init();
        startPos = transform.position;
        dashing = false;
        chargingDash = true;
        caster.ResetVelocity();
        caster.StopMoving();
        gameObject.layer = LayerMask.NameToLayer("IgnoreEntities");
        Executing = true;
    }

    public void OnFinishDash()
    {
        caster.ResetVelocity();
        caster.canMove = true;
        caster.applyFriction = true;
        gameObject.layer = LayerMask.NameToLayer("Entities");
        Executing = false;
    }

    public void SpawnDashEffect()
    {
        if (DashEffect != null)
            Instantiate(DashEffect, transform.position, Quaternion.identity);
    }

    public override bool conditionsMet(AbstractEnemyAI enemyAI)
    {
        target = GameManager.player == null ? transform.position : GameManager.player.transform.position;
        return canCast() && TargetInRange() && enemyAI.canSeePlayer;
    }

    public override void execute(AbstractEnemyAI enemyAI)
    {
        // If dashing check if we have travelled the dash distance and stop.
        if (Vector2.Distance(startPos, transform.position) >= DashDistance && dashing && Executing)
            OnFinishDash();

        // If already dashing dont dash again
        if (dashing || chargingDash)
            return;

        if (caster == null)
            return;

        lastCastTime = Time.time;

        // Target wasnt set
        if (target == Vector2.zero)
            Debug.Log("[" + caster.name + "] " + "DashAbility target was not set");

        UpdateDirection();

        // Disable friction and movement
        caster.canMove = false;
        caster.applyFriction = false;
        caster.ResetVelocity();
        caster.AddVelocity(direction, DashSpeed);

        dashing = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop dashing if we collide with something.
        if (Executing)
        {
            // Apply damage
            if (collision.gameObject.tag == "Player")
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.ApplyAttack(DashDamage, DashKnockBack, caster);
            }

            OnFinishDash();
        }
    }

    // Calculate the direction from this game object to the target position.
    public void UpdateDirection()
    {
        direction = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized;
    }

    public bool TargetInRange()
    {
        return (Vector2.Distance(transform.position, target) < DashDistance);
    }
}
