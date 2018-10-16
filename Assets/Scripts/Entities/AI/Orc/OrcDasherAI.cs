using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcDasherAI : AbstractEnemyAI
{
    private void Start() {
    }

    private void Update() {
        if (canSeePlayer) {
            targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
            MoveToTarget();
        } else {
            targetPosition = null;
        }
    }
}
