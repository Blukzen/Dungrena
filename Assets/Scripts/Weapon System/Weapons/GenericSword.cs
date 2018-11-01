using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSword : MeleeWeapon {

    public override void Attack() {
        currentDamage = attackDamage;
        currentManaCost = 0;

        if (!CanAttack())
            return;

        animator.Play(mainAttackAnimation);
    }

    public override void SecondaryAttack() {
        currentDamage = secondaryAttackDamage;
        currentManaCost = secondaryAttackManaCost;

        if (!CanSecondaryAttack())
            return;

        animator.Play(secondaryAttackAnimation);
    }
}
