using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSword : MeleeWeapon {

    public override void Attack() {
        currentDamage = attackDamage;
        MeleeAttack(mainAttackAnimation);
    }

    public override void SecondaryAttack() {
        Debug.Log("Secondary Attack");
        currentDamage = secondaryAttackDamage;
        MeleeAttack(secondaryAttackAnimation);
    }
}
