using UnityEngine;
using System.Collections;

public class AbstractEnemyAI : AbstractAstarAI, ISearcher 
{
    [HideInInspector]
    public bool canSeePlayer;

    public void canSeeTarget(bool canSee) {
        canSeePlayer = canSee;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
