using UnityEngine;
using System.Collections;

public interface ISearcher {
    void canSeeTarget(bool canSee);
    void canAttackTarget(bool canAttack);
}
