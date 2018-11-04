using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : AbstractProjectile
{
    private ParticleSystemRenderer[] particleRenderers;

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        foreach (var col in collision.contacts) 
        {
            var entity = col.collider.gameObject.GetComponent<AbstractEntity>();

            // Destroy projectile if not an entity.
            // TODO: Destroy event.
            if (!entity)
            {
                Destroy(gameObject);
                continue;
            }
            // Ignore collision if it is its own owner.
            else if (entity.Equals(entityOwner))
            {
                Physics2D.IgnoreCollision(entity.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
                continue;
            }

            else if (enemyProjectile && entity is AbstractEnemy)
            {
                Physics2D.IgnoreCollision(entity.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
                continue;
            }
            else
            {
                // Damage entity and destroy projectile.
                entity.ApplyAttack(Damage, knockback, entityOwner);
                Destroy(gameObject);
            }
        }
    }

    private void LateUpdate()
    {
        if (particleRenderers == null)
            particleRenderers = GetComponentsInChildren<ParticleSystemRenderer>();

        foreach (var fx in particleRenderers)
        {
            if (fx == null)
                continue;

            fx.sortingOrder = GetComponentInParent<SpriteRenderer>().sortingOrder - 1;
        }
    }
}
