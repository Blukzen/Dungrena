using UnityEngine;
using System.Collections;

public class StaffOfASpaztic : ProjectileWeapon
{
    public override void Attack()
    {
        currentDamage = attackDamage;
        currentManaCost = 0;

        if (CanAttack())
        {
            animator.Play(mainAttackAnimation);
        }
    }

    public override void SecondaryAttack()
    {
        currentDamage = secondaryAttackDamage;
        currentManaCost = secondaryAttackManaCost;

        if (CanSecondaryAttack())
        {
            animator.Play(abilityAttackAnimation);
        }
    }
}
