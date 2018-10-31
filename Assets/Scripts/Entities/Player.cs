using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractEntity
{
    public float maxMana;
    public float mana;

    public AbstractWeapon currentWeapon;
    [SerializeField]
    private AbstractWeapon defaultWeapon;
    private AbstractAbility ability;

    public float damageShakeAmount = 0.15f;

    private void Start() {
        mana = maxMana;
        ability = GetComponent<AbstractAbility>();

        if (currentWeapon == null)
        {
            //Instantiate(defaultWeapon, transform.position + new Vector3(Vector2.left.x * 5, 0, 0), Quaternion.identity);
        }
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0)) {
            if (currentWeapon != null) currentWeapon.Attack();
        } else if (Input.GetMouseButtonDown(1)) {
            if (currentWeapon != null)
                currentWeapon.SecondaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            pickupItem();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        SetMovement(direction);
        UpdatePhysics();    
        

        GetComponent<Animator>().SetFloat("Movement", direction.magnitude);
    }

    public override void DamageEffectPlay()
    {
        base.DamageEffectPlay();

        var CameraShake = Camera.main.GetComponent<CameraShake>();
        if (CameraShake != null)
            StartCoroutine(CameraShake.Shake(damageShakeAmount, 0.1f));
    }

    public bool UseMana(int amount) 
    {
        if (mana < amount)
            return false;

        mana -= amount;
        return true;
    }

    public void EquipWeapon(AbstractWeapon weapon)
    {
        if (currentWeapon != null)
            currentWeapon.Drop(weapon.transform.position);

        weapon.transform.parent = transform.Find("Weapon");
        currentWeapon = weapon;
    }
}
