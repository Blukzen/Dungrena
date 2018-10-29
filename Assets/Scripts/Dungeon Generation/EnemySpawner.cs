using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, ISpawner 
{
    [SerializeField]
    private SpawnList spawnList; // Potential entities that can spawn along with their spawn chance.

    private GameObject enemiesParent;

    private int maxEntities = 0;
    private int minEntities = 0;
    private int enemyMultiplier = 3;

    private Room room;

    public void OnEnable() 
    {
        enemiesParent = new GameObject();
        enemiesParent.transform.parent = this.transform;
        enemiesParent.name = "Enemies";
    }

    public void Spawn(Room _room) 
    {

        room = _room;
        maxEntities = room.difficulty * (enemyMultiplier + 2);
        minEntities = room.difficulty * (enemyMultiplier - 1);

        RNGSpawn();
    }

    private void RNGSpawn() 
    {
        // Pick a random number of entities to 
        var numberToSpawn = Random.Range(minEntities, maxEntities);
        var enemiesSpawned = 0;

        while (enemiesSpawned < numberToSpawn) {
            var chance = Random.value;
            var enemy = Instantiate(spawnList.GetRandEnemy(chance), room.RandomPointInRoom(), Quaternion.identity);
            enemy.transform.parent = enemiesParent.transform;

            enemiesSpawned++;
        }
    }

    public void KillAll() 
    {
        foreach (Transform enemy in enemiesParent.transform) {
            Destroy(enemy.gameObject);
        }
    }
}
