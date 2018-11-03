using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnList", menuName = "DungeonGen/SpawnList", order = 2)]
public class SpawnList : ScriptableObject
{
    public WeightedSpawnChance[] enemies;

    private int sum;

    private void Awake()
    {
        
    }

    public AbstractEntity GetRandEnemy()
    {
        //TODO: Optimize by having sum calculated on startup.
        sum = 0;
        foreach (var spawnChance in enemies)
        {
            sum += spawnChance.weight;
        }

        int min = 0, max = 0;
        var num = Random.Range(0, sum);

        for (int i = 0; i < enemies.Length; i++)
        {
            max += enemies[i].weight;

            if (num >= min && num <= max)
            {
                return enemies[i].entity;
            }

            min = max;
        }

        return null;
    }
}

[System.Serializable]
public class WeightedSpawnChance
{
    public AbstractEntity entity;
    public int weight;
}

