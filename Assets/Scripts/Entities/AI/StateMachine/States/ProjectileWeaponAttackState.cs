using UnityEngine;
using System.Collections;

public class ProjectileWeaponAttackState : AbstractEnemyState
{
    public float AttackRange;

    public override string Name { get { return "Projectile Weapon Attack"; } }

    public override void init()
    {
        base.init();
        Executing = true;
    }

    public override bool conditionsMet(AbstractEnemyAI enemyAI)
    {
        return enemyAI.CanAttackTarget();
    }

    public override void execute(AbstractEnemyAI enemyAI)
    {
        entity.StopMoving();

        if (entity.currentWeapon.attacking)
            return;

        if (entity.currentWeapon.CanSecondaryAttack())
        {
            entity.SecondaryAttack();
        }
        else
        {
            entity.Attack();
        }

        if (!enemyAI.CanAttackTarget())
            Executing = false;
    }

    public bool TargetInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, AttackRange, 1 << LayerMask.NameToLayer("Player"));

        GameObject player = null;

        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                player = collider.gameObject;
            }
        }

        return player != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
