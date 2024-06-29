using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;

    void Start()
    {
        InvokeRepeating("SpawnBlock", spawnInterval, spawnInterval);
    }

    void SpawnBlock()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(blockPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }
}
