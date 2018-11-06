using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcDasherAI : AbstractEnemyAI
{
    private IdleState idleState;
    private PatrolState patrolState;
    private ChaseState chaseState;
    private DashAttackState dashAttackState;

    private Animator animator;

    private void Start()
    {
        idleState = GetComponent<IdleState>();
        patrolState = GetComponent<PatrolState>();
        chaseState = GetComponent<ChaseState>();
        dashAttackState = GetComponent<DashAttackState>();

        animator = GetComponent<Animator>();

        currentState = idleState;
    }

    private void FixedUpdate() {
        // Dont update state if the current state is not yet finished
        if (!currentState.Executing)
            UpdateState();

        currentState.execute(this);

        entity.UpdatePhysics();

        if (currentState == idleState)
            animator.SetTrigger("Idle");

        if (currentState == patrolState || currentState == chaseState)
            animator.SetTrigger("Walking");

        if (currentState == dashAttackState)
            animator.SetTrigger("Dashing");
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

        if (dashAttackState.conditionsMet(this) && currentState.Name != dashAttackState.Name)
            newState = dashAttackState;

        if (newState != null && newState != currentState)
        {
            newState.init();
            lastState = currentState;
            currentState = newState;
        }
    }

    public override bool CanAttackTarget()
    {
        return dashAttackState.conditionsMet(this) && !attackBlocked;
    }
}
