using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    public Color damageEnemy;
    public Color damagePlayer;
    public Color heal;
    public Color manaRegen;

    private Text text;
    public void DamageEnemy(int num)
    {
        text = GetComponent<Text>();
        text.text = "-" + num;
        text.color = damageEnemy;
    }

    public void DamagePlayer(int num)
    {
        text = GetComponent<Text>();
        text.text = "-" + num;
        text.color = damagePlayer;
    }

    public void Heal(int num)
    {
        text = GetComponent<Text>();
        text.text = "+" + num;
        text.color = heal;
    }

    public void ManaRegen(int num)
    {
        text = GetComponent<Text>();
        text.text = "+" + num;
        text.color = manaRegen;
    }

    public void Cleanup()
    {
        Destroy(transform.parent.gameObject);
    }
}