using UnityEngine;
using System.Collections;

public abstract class AbstractEnemyState : MonoBehaviour, IEnemyState
{
    public virtual bool Executing { get; set; }
    public abstract string Name { get; }
    protected AbstractEnemyAI enemyAI;
    protected AbstractEnemy entity;

    protected virtual void Awake()
    {
        enemyAI = GetComponent<AbstractEnemyAI>();
        entity = GetComponent<AbstractEnemy>();
    }

    public abstract bool conditionsMet(AbstractEnemyAI enemyAI);
    public abstract void execute(AbstractEnemyAI enemyAI);

    public virtual void init() {}
}
