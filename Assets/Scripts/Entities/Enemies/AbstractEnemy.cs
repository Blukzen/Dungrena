using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : AbstractEntity
{
    public Player target;

    public float AbilityCooldown;
    private float lastAbilityAttack;

    [HideInInspector]
    public GameObject weaponHolder;
    [HideInInspector]
    public AbstractWeapon currentWeapon;
    [HideInInspector]
    public bool lookingAt = false;
    [HideInInspector]
    public Vector2 lookPos;

    public override void Awake()
    {
        base.Awake();
        target = GameManager.player;

        if (transform.Find("Weapon"))
            weaponHolder = transform.Find("Weapon").gameObject;
    }

    public override bool CanSecondaryAttack()
    {
        return Time.time - lastAbilityAttack >= AbilityCooldown && currentWeapon.CanAttack();
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

    public void SecondaryAttack()
    {
        if (currentWeapon == null)
        {
            Debug.Log("[" + name + "]" + "Has no weapon to attack with!");
            return;
        }

        currentWeapon.SecondaryAttack();
        lastAbilityAttack = Time.time;
    }

    protected override void UpdateSprite()
    {
        base.UpdateSprite();

        if (currentWeapon != null)
            currentWeapon.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder - 1;

        var newScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (lookingAt)
        {
            if (transform.position.x < lookPos.x)
                newScale.x = 1;
            else if (transform.position.x > lookPos.x)
                newScale.x = -1;

        }
        else
        {
            if (rb2d.velocity.x < -0.5)
                newScale.x = -1;
            else if (rb2d.velocity.x > 0.5)
                newScale.x = 1;
        }

        transform.localScale = newScale;
    }

    protected override IEnumerable FallingAnim()
    {
        float yShrinkRate = 0.009f;
        float xShrinkRate = 0.009f;
        float timeCount = 0;

        // In case flipped
        xShrinkRate *= transform.localScale.x;

        while (transform.localScale.y > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x - xShrinkRate, transform.localScale.y - yShrinkRate);
            transform.position = new Vector2(transform.position.x, transform.position.y - yShrinkRate);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - yShrinkRate);

            if (currentWeapon != null)
            {
                currentWeapon.GetComponent<SpriteRenderer>().color = sprite.color;
            }

            yield return new WaitForSeconds(0.1f);
            timeCount += Time.deltaTime;

            if (timeCount > 0.01)
            {
                sprite.sortingLayerName = "Default";
                sprite.sortingOrder = -10;
            }
        }

        FinishedFall();
    }

    public override void Damage(float amount)
    {
        base.Damage(amount);
        UIManager.PopupDamageEnemy(transform.position, (int)amount);
    }

    public override void Damage(float amount, AbstractEntity attacker)
    {
        base.Damage(amount, attacker);
        if (attacker is Player)
        {
            GameManager.score += (int)amount;
        }
        UIManager.PopupDamageEnemy(transform.position, (int)amount);
    }
    public override void Killed(AbstractEntity killer)
    {
        base.Killed(killer);
        if (killer is Player)
        {
            GameManager.player.Heal((int)maxHealth / 4);
            GameManager.score += (int)maxHealth;
        }
        Destroy(gameObject);
    }
}
