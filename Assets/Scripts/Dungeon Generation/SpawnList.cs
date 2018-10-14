using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnList", menuName = "DungeonGen/SpawnList", order = 2)]
public class SpawnList : ScriptableObject
{
    public AbstractEntity common;
    public AbstractEntity uncommon;
    public AbstractEntity rare;
    public AbstractEntity special;

    public AbstractEntity GetRandEnemy(float chance) {
        if (chance <= 0.15) {
            return special;
        } else if (chance <= 0.35) {
            return rare;
        } else if (chance <= 0.6) {
            return uncommon;
        } else {
            return common;
        }
    }
}

