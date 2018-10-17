using UnityEngine;

public class AbstractEnemyAI : AbstractAstarAI, ISearcher 
{
    [HideInInspector]
    public bool canSeePlayer;

    public void canSeeTarget(bool canSee) 
    {
        canSeePlayer = canSee;
    }
}
