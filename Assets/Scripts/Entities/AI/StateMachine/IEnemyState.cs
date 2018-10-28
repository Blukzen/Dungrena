using UnityEngine;
using System.Collections;

public interface IEnemyState 
{
    bool Executing { get; set; }
    string Name { get; }

    void init();
    void execute(AbstractEnemyAI enemyAI);
    bool conditionsMet(AbstractEnemyAI enemyAI);
}
