using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcDasherAI : AbstractEnemyAI
{
    private void FixedUpdate() {
        if (canSeePlayer) {
            targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
            AstarMoveToTarget();
        } else {
            targetPosition = null;
            entity.SetMovement(new Vector2());
        }

        entity.UpdatePhysics();
    }
}
