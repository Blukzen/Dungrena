using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DungeonGenerator dungeon = (DungeonGenerator) target;

        EditorGUILayout.LabelField("Dungeon Settings", EditorStyles.boldLabel);

        dungeon.roomHeight = EditorGUILayout.IntField("Room Height", dungeon.roomHeight);
        dungeon.roomWidth = EditorGUILayout.IntField("Room Width", dungeon.roomWidth);
        dungeon.roomSpacing = EditorGUILayout.IntField("Room Spacing", dungeon.roomSpacing);
        dungeon.roomConnectionWidth = EditorGUILayout.IntField("Room Connection Width", dungeon.roomConnectionWidth);
        dungeon.dungeonSize = EditorGUILayout.IntField("Dungeon Size", dungeon.dungeonSize);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Dungeon Tiles", EditorStyles.boldLabel);

        dungeon.wallTile = (RuleTile)EditorGUILayout.ObjectField("Wall Tile", dungeon.wallTile, typeof(RuleTile), false);
        dungeon.floorTile = (WeightedRandomTile)EditorGUILayout.ObjectField("Floor Tile", dungeon.floorTile, typeof(WeightedRandomTile), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("Regenerate Dungeon") && Application.isPlaying)
        {
            dungeon.Regenerate();
        }
    }
}