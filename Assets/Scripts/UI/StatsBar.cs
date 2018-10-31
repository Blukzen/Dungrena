using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    public Transform bar;

    public void UpdateBar(float current, float max)
    {
        if (bar == null)
        {
            Debug.LogWarning("[UI] No bar attatched to stats bar " + name);
            return;
        }

        bar.localScale = new Vector3(current / max, 1, 1);
    }
}
