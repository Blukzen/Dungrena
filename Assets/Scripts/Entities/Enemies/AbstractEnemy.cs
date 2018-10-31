using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : AbstractEntity
{
    private AbstractAbility attack;
    public Player target;

    [HideInInspector]
    public AbstractWeapon currentWeapon;

    private void Start()
    {
        attack = GetComponent<AbstractAbility>();
        target = GameManager.player;
    }

    public void Attack()
    {
        if (currentWeapon == null)
        {
            Debug.Log("[" + name + "] " + "Has no weapon to attack with!");
            return;
        }

        currentWeapon.Attack();
    }

    protected override void UpdateSprite()
    {
        base.UpdateSprite();
        if (currentWeapon != null)
            currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder - 1;

        var newScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (rb2d.velocity.x < -0.5)
        {
            newScale.x = -1;
        }
        else if (rb2d.velocity.x > 0.5)
        {
            newScale.x = 1;
        }

        transform.localScale = newScale;
    }

    // TODO: Death effect
    public override void Killed()
    {
        base.Killed();
        Destroy(gameObject);
    }
}
