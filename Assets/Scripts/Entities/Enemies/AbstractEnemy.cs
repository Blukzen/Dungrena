using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : AbstractEntity
{
    private AbstractAbility attack;
    public Player target;

    private void Start()
    {
        attack = GetComponent<AbstractAbility>();
        target = GameManager.player;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**public override void UpdatePhysics()
    {
        // TODO: friction and acceleration multipliers
        var _friction = friction;
        var _acceleration = acceleration;

        Vector2 newVel = rb2d.velocity;

        // Left
        if (moveDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1);
            // Apply acceleration left
            if (newVel.x > 0)
                newVel.x = Approach(newVel.x, 0, _friction);
            newVel.x = Approach(newVel.x, moveDirection.x * maxSpeed, _acceleration);
        }

        // Right
        if (moveDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1);
            // Apply acceleration right
            if (newVel.x < 0)
                newVel.x = Approach(newVel.x, 0, _friction);
            newVel.x = Approach(newVel.x, moveDirection.x * maxSpeed, _acceleration);
        }

        // Down
        if (moveDirection.y < 0)
        {
            // Apply acceleration down
            if (newVel.y > 0)
                newVel.y = Approach(newVel.y, 0, _friction);
            newVel.y = Approach(newVel.y, moveDirection.y * maxSpeed, _acceleration);
        }

        // Up
        if (moveDirection.y > 0)
        {
            // Apply acceleration up
            if (newVel.y < 0)
                newVel.y = Approach(newVel.y, 0, _friction);
            newVel.y = Approach(newVel.y, moveDirection.y * maxSpeed, _friction);
        }


        // Friction when not moving
        if (moveDirection.x == 0)
            newVel.x = Approach(newVel.x, 0, _friction);
        if (moveDirection.y == 0)
            newVel.y = Approach(newVel.y, 0, _friction);

        rb2d.velocity = newVel;
    }*/

    public void Attack()
    {
        if (attack == null)
        {
            Debug.Log("[" + gameObject.name + "] " + " Has no ability to cast");
            return;
        }

        attack.cast();
    }

    // TODO: Death effect
    public override void Killed()
    {
        base.Killed();
        Destroy(gameObject);
    }
}
