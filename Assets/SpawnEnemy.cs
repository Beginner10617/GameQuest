using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public List<Transform> spawnPositions;
    public GameObject enemyPrefab;

    int index = 0;
    public void Spawn()
    {
        GameObject Enemy = Instantiate(enemyPrefab, spawnPositions[index++].position, Quaternion.identity);
        

    }
}
