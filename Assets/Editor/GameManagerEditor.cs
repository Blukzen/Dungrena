using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager gameManager = (GameManager) target;

        if (GUILayout.Button("Regenerate Dungeon"))
        {
            gameManager.RegenerateDungeon();
        }
    }
}
