using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireball : AbstractProjectile
{
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        foreach (var col in collision.contacts) 
        {
            var entity = col.collider.gameObject.GetComponent<AbstractEntity>();

            // Destroy projectile if not an entity.
            // TODO: Destroy event.
            if (!entity) {
                Destroy(gameObject);
                continue;
            }
            // Ignore collision if it is its own owner.
            else if (entity.Equals(Owner)) 
            {
                Physics2D.IgnoreCollision(entity.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
                continue;
            } 

            // Damage entity and destroy projectile.
            entity.Damage(Damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        //var entity = collision.collider.gameObject.GetComponent<AbstractEntity>();

        var entity = collision.GetComponent<AbstractEntity>();

        // Destroy projectile if not an entity.
        // TODO: Destroy event.
        if (!entity) 
        {
            Destroy(gameObject);
            return;
        }
        // Ignore collision if it is its own owner.
        else if (entity.Equals(Owner)) 
        {
            Physics2D.IgnoreCollision(entity.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
            return;
        }

        // Damage entity and destroy projectile.
        entity.Damage(Damage);
        Destroy(gameObject);
    }
}
