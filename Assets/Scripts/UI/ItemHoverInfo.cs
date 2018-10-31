using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemHoverInfo : MonoBehaviour
{
    public Text ItemName;
    public Text ItemDamage;
    public Text ItemSpeed;
    public Text ItemAbilityDamage;
    public Text ItemManaCost;

    public void ShowWeaponInfo(AbstractWeapon weapon)
    {
        ItemName.text = weapon.ItemName;
        ItemDamage.text = weapon.attackDamage.ToString();
        ItemSpeed.text = weapon.attackSpeed.ToString();
        ItemAbilityDamage.text = weapon.secondaryAttackDamage.ToString();
        ItemManaCost.text = weapon.secondaryAttackManaCost.ToString();

        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 100, 0);

        Rect rect = GetComponent<RectTransform>().rect;
    }
}
