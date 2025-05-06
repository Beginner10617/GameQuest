using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public GameObject phoneObject; // Assign the phone prefab separately
    public float spawnInterval = 1f;
    public float xMin = -8f, xMax = 8f;
    [SerializeField] public RectTransform effectSpawnPoint;
    public int spawnCount = 0;
    public int maxSpawns = 1000;
    public ObjectsFallingGameStart ObjectsFallingGameStart;
    public GameObject FlipPhoneUI;
    public Transform playerTransform; // Assign in inspector
    public gameTimer _gameTimer;
    [Range(0f, 1f)]
    public float playerBiasStrength = 0.8f; // 0 = full random, 1 = fully player focused

    private void OnEnable()
    {
        FlipPhoneUI?.SetActive(true);
        InvokeRepeating("SpawnObject", 1f, spawnInterval);
    }

    private void OnDisable()
    {
        FlipPhoneUI?.SetActive(false);
        ObjectsFallingGameStart.StopGame();
        CancelInvoke("SpawnObject");
    }

    void SpawnObject()
    {
        if (spawnCount >= maxSpawns)
        {
            _gameTimer.GameOver(1);
        }

        GameObject objectToSpawn = ChooseBiasedObject();
        float biasedX = GetBiasedXPosition();
        float randomX = transform.position.x + Random.Range(xMin, xMax);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);
        GameObject g = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        try { g.GetComponent<decreaseTimer>().effectSpawnPoint = effectSpawnPoint; }
        catch { }
        Destroy(g, 5f);

        spawnCount++;
    }

    GameObject ChooseBiasedObject()
    {
        // Increase the chance of spawning the phone
        float phoneSpawnChance = 0.25f; // 50% chance to spawn phone

        if (Random.value < phoneSpawnChance)
        {
            return phoneObject;
        }
        else
        {
            int index = Random.Range(0, objectsToSpawn.Length);
            return objectsToSpawn[index];
        }
    }

    float GetBiasedXPosition()
    {
        float playerX = playerTransform != null ? playerTransform.position.x : 0;
        
        float randomX = Random.Range(xMin, xMax);
        float biasedX = Mathf.Lerp(transform.position.x + randomX, playerX, playerBiasStrength);
        //Debug.Log(biasedX);
        return Mathf.Clamp(biasedX, xMin, xMax);
    }
}
