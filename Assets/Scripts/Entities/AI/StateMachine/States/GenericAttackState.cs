using UnityEngine;
using System.Collections;

public class GenericAttackState : AbstractEnemyState
{
    public override string Name { get { return "Attack"; } }

    public override bool conditionsMet(AbstractEnemyAI enemyAI)
    {
        throw new System.NotImplementedException();
    }

    public override void execute(AbstractEnemyAI enemyAI)
    {
        throw new System.NotImplementedException();
    }
}
