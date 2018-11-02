using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    protected static string TAG = "[UIManager] ";
    protected static ItemHoverInfo itemInfo;
    protected static StatsBar healthBar;
    protected static StatsBar manaBar;
    public static LoadingScreen loadingScreen;

    public ItemHoverInfo _itemInfo;
    public StatsBar _healthBar;
    public StatsBar _manaBar;
    public LoadingScreen _loadingScreen;

    private void Start()
    {
        itemInfo = _itemInfo;
        healthBar = _healthBar;
        manaBar = _manaBar;
        loadingScreen = _loadingScreen;

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

    public static void UpdateMana(float current, float max)
    {
        if (manaBar == null)
        {
            Debug.Log(TAG + "UI Mana bar does not exist");
        }

        manaBar.UpdateBar(current, max);
    }

    public static void UpdateHealth(float current, float max)
    {
        if (healthBar == null)
        {
            Debug.Log(TAG + "UI Health bar does not exist");
        }

        healthBar.UpdateBar(current, max);
    }
}
