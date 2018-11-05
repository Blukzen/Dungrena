using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New ObjectList", menuName = "DungeonGen/ObjectList", order = 2)]
public class ObjectGenList : ScriptableObject
{
    public ObjectSpawnData[] objects;
}

[System.Serializable]
public class ObjectSpawnData
{
    [HideInInspector]
    public int NumInDungeon = 0;

    public PrefabTile tile;
    public float chance;
    public int MinPerRoom;
    public int MaxPerRoom;
    public int MaxPerDungeon;
}
