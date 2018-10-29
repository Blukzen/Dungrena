using UnityEngine;
using System.Collections;

public class DashAbility : AbstractAbility 
{

    public float DashPower = 25;
    public float DashDistance = 5;

    private Vector2 direction;
    private Vector2 startPos;
    private bool dashing = false;

    public override void cast() 
    {
        if (caster == null)
            return;

        if (!canCast())
            return;

        lastCastTime = Time.time;

        if (target == null || target == Vector2.zero)
        {
            Debug.Log("[" + caster.name + "] " + "DashAbility target was not set");
        }

        UpdateDirection();

        // Disable friction and movement
        caster.canMove = false;
        caster.applyFriction = false;

        dashing = true;
        startPos = transform.position;
        caster.AddVelocity(direction, DashPower);

        // Reset target
        target = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop dashing if we collide with something.
        if (dashing)
        {
            caster.ResetVelocity();
            caster.canMove = true;
            caster.applyFriction = true;
            dashing = false;
        }
    }

    private void Update()
    {
        // If dashing check if we have travelled the dash distance and stop.
        if (dashing)
        {
            if (Vector2.Distance(startPos, transform.position) >= DashDistance)
            {
                caster.ResetVelocity();
                caster.canMove = true;
                caster.applyFriction = true;
                dashing = false;
            }
        }
    }

    // Calculate the direction from this game object to the target position.
    public void UpdateDirection()
    {
        direction = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized;
    }
}
