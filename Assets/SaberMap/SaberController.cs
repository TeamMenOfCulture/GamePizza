using UnityEngine;

public class SaberController : MonoBehaviour
{
    public Transform head; // Reference to the VR head/camera
    public float distance = 2f; // Distance from the head

    void Update()
    {
        Vector3 targetPosition = head.position + head.forward * distance;
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
        transform.rotation = head.rotation;
    }
}
