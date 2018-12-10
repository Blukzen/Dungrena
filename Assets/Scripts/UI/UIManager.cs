using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    protected static string TAG = "[UIManager] ";
    protected static ItemHoverInfo itemInfo;
    protected static StatsBar healthBar;
    protected static StatsBar manaBar;
    protected static GameObject popupNumberPrefab;
    public static LoadingScreen loadingScreen;
    public static GameoverScreen gameoverScreen;

    public ItemHoverInfo _itemInfo;
    public StatsBar _healthBar;
    public StatsBar _manaBar;
    public LoadingScreen _loadingScreen;
    public GameoverScreen _gameoverScreen;

    private void Start()
    {
        itemInfo = _itemInfo;
        healthBar = _healthBar;
        manaBar = _manaBar;
        loadingScreen = _loadingScreen;
        gameoverScreen = _gameoverScreen;

        popupNumberPrefab = Resources.Load<GameObject>("Prefabs/Effects/Popup Number UI");

        if (itemInfo != null && itemInfo.gameObject.activeSelf)
            itemInfo.gameObject.SetActive(false);
    }

    public static void ShowHUD()
    {
        if (healthBar == null)
            return;

        healthBar.gameObject.SetActive(true);
        manaBar.gameObject.SetActive(true);
    }

    public static void HideHUD()
    {
        if (healthBar == null)
            return;

        healthBar.gameObject.SetActive(false);
        manaBar.gameObject.SetActive(false);
    }

    public static void ResetHUD()
    {
        healthBar.UpdateBar(1, 1);
        manaBar.UpdateBar(1, 1);
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

    public static void GameOver()
    {
        gameoverScreen.gameObject.SetActive(true);
    }

    public static void BackToMenu()
    {
        gameoverScreen.FadeOut();
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

    public static void PopupDamageEnemy(Vector2 position, int amount)
    {
        var screenPos = Camera.main.WorldToScreenPoint(position);
        var popup = Instantiate(popupNumberPrefab, screenPos, Quaternion.identity);
        popup.transform.SetParent(UIManager.instance.transform);
        popup.GetComponentInChildren<PopupText>().DamageEnemy(amount);
    }

    public static void PopupDamagePlayer(Vector2 position, int amount)
    {
        var screenPos = Camera.main.WorldToScreenPoint(position);
        var popup = Instantiate(popupNumberPrefab, screenPos, Quaternion.identity);
        popup.transform.SetParent(UIManager.instance.transform);
        popup.GetComponentInChildren<PopupText>().DamagePlayer(amount);
    }

    public static void PopupHeal(Vector2 position, int amount)
    {
        var screenPos = Camera.main.WorldToScreenPoint(position);
        var popup = Instantiate(popupNumberPrefab, screenPos, Quaternion.identity);
        popup.transform.SetParent(UIManager.instance.transform);
        popup.GetComponentInChildren<PopupText>().Heal(amount);
    }

    public static void PopupManaHeal(Vector2 position, int amount)
    {
        var screenPos = Camera.main.WorldToScreenPoint(position);
        var popup = Instantiate(popupNumberPrefab, screenPos, Quaternion.identity);
        popup.transform.SetParent(UIManager.instance.transform);
        popup.GetComponentInChildren<PopupText>().ManaRegen(amount);
    }
}
