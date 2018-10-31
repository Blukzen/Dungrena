using UnityEngine;
using System.Collections;

public class WeaponAttackState : AbstractEnemyState
{
    public override string Name { get { return "Weapon Attack State";  } }
    public Rect attackBox;

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
        enemyAI.Attack();
        Executing = false;
    }

    public bool TargetInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (attackBox.center.x * transform.localScale.x), transform.position.y + attackBox.center.y), attackBox.size, 0, 1 << LayerMask.NameToLayer("Player"));

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
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (attackBox.center.x * transform.localScale.x), transform.position.y + attackBox.center.y), attackBox.size);
    }
}
