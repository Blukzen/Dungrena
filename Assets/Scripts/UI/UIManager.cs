using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    protected static string TAG = "[UIManager] ";
    protected static ItemHoverInfo itemInfo;

    public ItemHoverInfo _itemInfo;

    private void Start()
    {
        itemInfo = _itemInfo;

        if (itemInfo != null && itemInfo.gameObject.activeSelf)
            itemInfo.gameObject.SetActive(false);
    }

    public static void ShowWeaponInfo(AbstractWeapon weapon)
    {
        if (itemInfo == null)
        {
            Debug.LogError(TAG + "No item hover info has been attatched");
            return;
        }

        itemInfo.gameObject.SetActive(true);
        itemInfo.ShowWeaponInfo(weapon);
    }

    public static void HideWeaponInfo()
    {
        if (itemInfo == null)
        {
            Debug.LogError(TAG + "No item hover info has been attatched");
            return;
        }

        itemInfo.gameObject.SetActive(false);
    }
}
