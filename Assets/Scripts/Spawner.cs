using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public float spawnInterval = 1f;
    public float xMin = -8f, xMax = 8f;
    [SerializeField] public RectTransform effectSpawnPoint;
    public int spawnCount = 0;
    public int maxSpawns = 30;
    public ObjectsFallingGameStart ObjectsFallingGameStart;
    private void OnEnable()
    {
        InvokeRepeating("SpawnObject", 1f, spawnInterval);
    }
    void SpawnObject()
    {
        if(spawnCount <= maxSpawns)
        {
            int index = Random.Range(0, objectsToSpawn.Length);
            Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(xMin, xMax), transform.position.y, 0);
            GameObject g = Instantiate(objectsToSpawn[index], spawnPos, Quaternion.identity);
            g.GetComponent<decreaseTimer>().effectSpawnPoint = effectSpawnPoint;
            Destroy(g, 5f);
            spawnCount++;
        }
        else
        {
            ObjectsFallingGameStart.StopGame();
            CancelInvoke("SpawnObject");
        }
        
    }
}
