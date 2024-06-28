using UnityEngine;

public class EarthquakeSound : MonoBehaviour
{
    public AudioClip earthquakeClip;
    public float maxDistance = 10f;

    private AudioSource audioSource;
    private Transform playerTransform;

    void Start()
    {
        // Get the player GameObject by tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Assign the player's transform
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the specified tag.");
        }

        // Get the AudioSource component on the ground object
        audioSource = GetComponent<AudioSource>();

        // Set the earthquake audio clip
        audioSource.clip = earthquakeClip;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return; // Don't proceed if the playerTransform is not assigned
        }

        // Calculate the distance from the player to the ground object
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Adjust the volume based on the distance
        float normalizedVolume = 0.8f - Mathf.Clamp01(distanceToPlayer / maxDistance);
        audioSource.volume = normalizedVolume;

        // Play the earthquake sound
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
