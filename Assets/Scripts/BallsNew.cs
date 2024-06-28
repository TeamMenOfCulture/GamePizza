using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsNew : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnInterval = 2f;
    public float colliderOffTime = 5f;

    void Start()
    {
        // Start spawning balls
        InvokeRepeating("SpawnBall", 0f, spawnInterval);
    }

    void SpawnBall()
    {
        // Randomly choose one among the spawn points
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate a new ball at the chosen spawn point
        GameObject ball = Instantiate(ballPrefab, randomSpawnPoint.position, Quaternion.identity);
    }

}