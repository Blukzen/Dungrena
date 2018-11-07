using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    public RectTransform bar;
    public float fullWidth;
    public void UpdateBar(float current, float max)
    {
        if (bar == null)
        {
            Debug.LogWarning("[UI] No bar attatched to stats bar " + name);
            return;
        }

        bar.sizeDelta = new Vector2(current/max * fullWidth, bar.sizeDelta.y);
    }
}
