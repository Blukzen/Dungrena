using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireball : AbstractProjectile
{
    private void OnCollisionEnter2D(Collision2D collision) {
        foreach (var col in collision.contacts) {
            var entity = col.collider.gameObject.GetComponent<AbstractEntity>();
            if (!entity || entity.Equals(Owner)) {
                Destroy(gameObject);
                continue;
            }

            entity.Damage(Damage);
            Destroy(gameObject);
        }
    }
}
