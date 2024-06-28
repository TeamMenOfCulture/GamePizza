using UnityEngine;

public class RemoveBoundaries : MonoBehaviour
{
    void Start()
    {
        // Remove game objects named "Boundary1"
        GameObject[] boundaries1 = GameObject.FindGameObjectsWithTag("GUARD");
        foreach (GameObject boundary1 in boundaries1)
        {
            Destroy(boundary1);
        }


    }
}
