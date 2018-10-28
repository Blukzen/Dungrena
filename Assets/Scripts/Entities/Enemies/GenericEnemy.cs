using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : AbstractEntity {

    private AbstractAbility attack;
	
	// Update is called once per frame
	void Update ()
    {
        //UpdatePhysics();
	}

    public override void UpdatePhysics()
    {
        // TODO: friction and acceleration multipliers
        var _friction = friction;
        var _acceleration = acceleration;

        Vector2 newVel = rb2d.velocity;

        // Left
        if (moveDirection.x < 0)
        {
            // Apply acceleration left
            if (newVel.x > 0)
                newVel.x = Approach(newVel.x, 0, _friction);
            newVel.x = Approach(newVel.x, moveDirection.x * maxSpeed, _acceleration);
        }

        // Right
        if (moveDirection.x > 0)
        {
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
    }

    void Attack()
    {
        attack.cast(this);
    }
}
