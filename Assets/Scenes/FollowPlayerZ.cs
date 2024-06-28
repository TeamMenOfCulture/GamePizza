using UnityEngine;

public class FollowPlayerZ : MonoBehaviour
{
    public string playerTag = "Player";

    private Transform playerTransform;

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
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return; // Don't proceed if the playerTransform is not assigned
        }

        // Keep the current Y and X positions
        float currentY = transform.position.y;
        float currentX = transform.position.x;

        // Set the position based on the player's position, keeping Y and X frozen
        transform.position = new Vector3(currentX, currentY, playerTransform.position.z);
    }
}