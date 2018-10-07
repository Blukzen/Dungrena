using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : AbstractWeapon {

    [SerializeField]
    private int projectileSpeed;
    [SerializeField]
    private AbstractProjectile projectile;
    [SerializeField]
    private Transform firePosition;

    public override void Attack() { Shoot(); }

    public override void AttackAnimation() 
    {
    }

    public void Shoot() 
    {
        var proj = Instantiate(projectile, firePosition.transform.position, DirectionToMouse());
        proj.Speed = projectileSpeed;
        proj.Owner = transform.GetComponentInParent<AbstractEntity>();
        proj.Damage = GetDamage();
    }

    private Quaternion DirectionToMouse() {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePosition.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
