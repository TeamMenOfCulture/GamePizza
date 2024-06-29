using UnityEngine;

public class SaberController : MonoBehaviour
{
    public Transform head;
    public float distance = 2f;

    void Update()
    {
        // Position the saber in front of the head
        Vector3 forward = head.forward;
        forward.y = 0; // Keep the saber horizontal
        forward.Normalize();
        transform.position = head.position + forward * distance;
        transform.rotation = head.rotation;
    }
}
