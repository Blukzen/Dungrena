using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcDasherAI : AbstractEnemyAI
{
    [HideInInspector]
    public IdleState idleState;
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public ChaseState chaseState;

    private void Start()
    {
        idleState = GetComponent<IdleState>();
        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        currentState = idleState;
    }

    private void FixedUpdate() {
        // Dont update state if the current state is not yet finished
        if (!currentState.Executing)
            UpdateState();

        currentState.execute(this);

        /**
        if (canSeePlayer) {
            targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            AstarMoveToTarget();
        } else {
            targetPosition = Vector2.zero;
            entity.SetMovement(new Vector2());
        }**/

       // entity.UpdatePhysics();
    }

    private void UpdateState()
    {

        AbstractEnemyState newState = null;

        if (idleState.conditionsMet(this) && currentState.Name != idleState.Name)
        {
            newState = idleState;
        }

        if (patrolState.conditionsMet(this) && currentState.Name != patrolState.Name)
        {
            newState = patrolState;
        }

        if (chaseState.conditionsMet(this) && currentState.Name != chaseState.Name)
            newState = chaseState;

        if (newState != null && newState != currentState)
        {
            newState.init();
            lastState = currentState;
            currentState = newState;
        }
    }
}
