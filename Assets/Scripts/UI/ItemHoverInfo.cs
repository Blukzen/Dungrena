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
        ClampToWindow();
    }

    void ClampToWindow()
    {
        var parentRectTransform = transform.parent.GetComponent<RectTransform>();
        var panelRectTransform = GetComponent<RectTransform>();

        Vector3 pos = panelRectTransform.localPosition;

        Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
        Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;

        pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);

        panelRectTransform.localPosition = pos;
    }
}
