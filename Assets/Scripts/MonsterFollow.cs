using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterFollow : MonoBehaviour
{
    public string playerTag = "Player";
    public float moveSpeed = 5f;
    public float originalMoveSpeed;
    public float rotationSpeed = 5f; // Added rotation speed
    public float stoppingDistance = 5f;

    private Transform playerTransform;
    private Animator monsterAnimator;

    public AudioClip leftFootstepClip;
    public AudioClip rightFootstepClip;
    public AudioClip roarClip;

    private AudioSource leftFootstepAudioSource;
    private AudioSource rightFootstepAudioSource;
    public AudioSource monsterAudioSource;

    public float footstepInterval = 0.1f;
    private float nextFootstepTime = 0f;

    private bool hasRoared = false;
    public float speedReccoveryTimeFactor = 0.1f;


    void Start()
    {
        originalMoveSpeed = moveSpeed;
        // SlowDownIntensity = 1f;
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

        // Get the Animator component on the monster
        monsterAnimator = GetComponent<Animator>();

        // Create separate AudioSources for left and right foot
        leftFootstepAudioSource = CreateAudioSource("LeftFootstepAudioSource");
        rightFootstepAudioSource = CreateAudioSource("RightFootstepAudioSource");
        leftFootstepAudioSource.volume = 0.5f;
        rightFootstepAudioSource.volume = 0.5f;

        // Get the AudioSource component on the monster
        monsterAudioSource = GetComponent<AudioSource>();
    }

    AudioSource CreateAudioSource(string name)
    {
        // Create an empty GameObject with an AudioSource component
        GameObject audioSourceObject = new GameObject(name);
        audioSourceObject.transform.parent = transform;
        audioSourceObject.transform.localPosition = Vector3.zero;

        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f; // Set to 3D audio

        return audioSource;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return; // Don't proceed if the playerTransform is not assigned
        }
        if (moveSpeed < originalMoveSpeed)
        {
            moveSpeed += Time.deltaTime * speedReccoveryTimeFactor;
        }

        // Calculate the distance from the monster to the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < stoppingDistance)
        {
            // Disable the player's movement script
            RunnerMovement playerMovement = playerTransform.GetComponent<RunnerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            // Look at each other
            Quaternion initialPlayerRotation = playerTransform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(playerTransform.position - transform.position);
            Quaternion targetRotationPlayer = Quaternion.LookRotation(transform.position - playerTransform.position);

            // Set y components to zero
            targetRotation.x = 0f;
            targetRotation.z = 0f;
            targetRotationPlayer.x = 0f;
            targetRotationPlayer.z = 0f;

            targetRotationPlayer.y = -(targetRotation.y - initialPlayerRotation.y);
            // Apply rotations
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotationPlayer, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Trigger the roar animation only once
            if (!hasRoared)
            {
                monsterAnimator.SetTrigger("Roar");
                PlayAudio(roarClip, monsterAudioSource);
                hasRoared = true;
            }

            // Stop the monster's movement
            monsterAnimator.SetFloat("MovementSpeed", 0f);

            // Trigger killing animation
            monsterAnimator.SetTrigger("KillPlayer");

            Invoke("GameOverB", 8f);
        }
        else
        {
            // Move the monster towards the player's position
            MoveTowardsPlayer();

            // Play footstep audio with a time interval
            if (Time.time > nextFootstepTime)
            {
                // Alternate between left and right footstep sounds
                if (monsterAnimator.GetBool("IsLeftFoot"))
                {
                    PlayAudio(leftFootstepClip, leftFootstepAudioSource);
                }
                else
                {
                    PlayAudio(rightFootstepClip, rightFootstepAudioSource);
                }

                // Toggle the foot boolean for the next iteration
                monsterAnimator.SetBool("IsLeftFoot", !monsterAnimator.GetBool("IsLeftFoot"));

                nextFootstepTime = Time.time + footstepInterval;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction from the monster to the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Move the monster in the calculated direction
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Set the y position to 0.59 (or any desired value)
        Vector3 newPosition = transform.position;
        newPosition.y = 0.59f;
        transform.position = newPosition;

        // Set the movement parameter in the Animator based on movement speed
        monsterAnimator.SetFloat("MovementSpeed", Mathf.Abs(moveSpeed));
    }

    void PlayAudio(AudioClip clip, AudioSource audioSource)
    {
        // Play the provided audio clip
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    public void GameOverB()
    {
        // Load the next scene (you can replace this with the appropriate scene index or name)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void SlowDown(float monsterSlowdown)
    {
        moveSpeed -= monsterSlowdown;
    }


    // public void RecoverSpeed()
    // {
    //     SlowDownIntensity = 1f;
    // }
}
