using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : AbstractEnemy
{
    public override void EquipItem(AbstractWeapon weapon)
    {
        if (currentWeapon != null)
            currentWeapon.Drop();

        currentWeapon = weapon;
        currentWeapon.Pickup(this);
        currentWeapon.transform.parent = transform.Find("Weapon");
        currentWeapon.transform.localScale = new Vector3(1, 1, 1);
    }
}
