using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public string playerTag = "Player"; // Tag of the player GameObject
    public float maxShakeDistance = 10f;
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.5f;

    private Transform playerTransform;
    private Vector3 originalCameraPosition;
    private bool isShaking = false;

    void Start()
    {
        // Find the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            // Assign the player's transform
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the specified tag.");
        }

        // Save the original camera position
        originalCameraPosition = Camera.main.transform.localPosition;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return; // Don't proceed if the playerTransform is not assigned
        }

        // Calculate the distance between the player and the shaking object
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Adjust shake intensity based on distance
        float adjustedShakeIntensity = Mathf.Lerp(0f, shakeIntensity, Mathf.Clamp01(distance / maxShakeDistance));

        // Trigger the shake if the distance is within the threshold
        if (distance <= maxShakeDistance && !isShaking)
        {
            StartCoroutine(ShakeCamera(adjustedShakeIntensity, shakeDuration));
        }
    }

    IEnumerator ShakeCamera(float intensity, float duration)
    {
        isShaking = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Randomize the camera position within a sphere
            Vector3 randomOffset = Random.insideUnitSphere * intensity;

            // Apply the shake effect to the camera
            Camera.main.transform.localPosition = originalCameraPosition + randomOffset;

            // Increment the elapsed time
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Reset the camera position
        Camera.main.transform.localPosition = originalCameraPosition;

        isShaking = false;
    }
}
