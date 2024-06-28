using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{ 
    public GameObject[] tilePrefabs;
    public float xSpawn = 9.9f;
    public float ySpawn = 0.593475f;
    public float zSpawn = 25;
    public float tileLength = 100;
    public int numberOfTiles = 2;
    public Transform playerTransform;
    private List<GameObject> activeTiles = new List<GameObject>();
    public float offset = 150f;

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            if (i == 0)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile(Random.Range(1, tilePrefabs.Length));
            }
        }

    }

    void Update()
    {
        if (playerTransform.position.z - offset > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(1, tilePrefabs.Length));
            DeleteTile();
        }
    }

    public void SpawnTile(int tileIndex)
    {
        GameObject go = Instantiate(tilePrefabs[tileIndex], new Vector3(xSpawn, ySpawn, zSpawn), transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLength;
    }
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
