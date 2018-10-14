using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileWeapon : AbstractWeapon 
{

    [SerializeField]
    private int projectileSpeed;
    [SerializeField]
    private AbstractProjectile projectile;
    [SerializeField]
    private Transform firePosition;

    /**public override void Attack() 
    {
        if (!CanAttack())
            return;

        lastAttackTime = Time.time;

        Shoot(0);
    }

    // Shoots projectiles in 360 circle
    public override void SecondaryAttack(Player player) 
    {
        if (!player.UseMana(secondaryAttackManaCost))
            return;

        int angle = 0;

        Shoot(0);
        Shoot(angle += 36);
        Shoot(angle += 36);
        Shoot(angle += 36);
        Shoot(angle += 36);
        Shoot(angle += 36);
        Shoot(angle += 36);
        Shoot(angle += 36);
        Shoot(angle += 36);
        Shoot(angle += 36);
    }

    // Dir the direction of the bullet where 0 is straight ahead and 90 is degree's clockwise.
    public void Shoot(int dir) 
    {
        var proj = Instantiate(projectile, firePosition.transform.position, DirectionToMouse(dir));
        proj.Owner = transform.parent.parent.GetComponent<AbstractEntity>();
        proj.Speed = projectileSpeed;
        proj.Damage = GetDamage();
    }**/

    protected Quaternion DirectionToMouse(int rot) 
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90 + rot);
    }
}
