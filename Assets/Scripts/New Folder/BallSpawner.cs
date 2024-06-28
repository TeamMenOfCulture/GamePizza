using System.Collections;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject ballPrefab;

    void Start()
    {
        StartCoroutine(SpawnBalls());
    }

    IEnumerator SpawnBalls()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            float randomZ = Random.Range(playerTransform.position.z, playerTransform.position.z + 150f);
            Vector3 spawnPosition = new Vector3(0f, 10f, randomZ);

            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
