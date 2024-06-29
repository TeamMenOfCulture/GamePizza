using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class RunnerMovement : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float speed;
    public float rotationLerpSpeed = 0.1f;
    public float jumpForce = 5f;
    public float gravity = 9.8f; // Custom gravity value
    public float groundCheckDistance = 0.2f;

    private Rigidbody rb;
    private Transform vrCamera;
    private bool isGrounded;

    private bool isJumping;
    private float currentJumpForce;
    private bool isMovementEnabled = true;
    private string lastScoreKey = "LatestScore";
    public float offsetScore = 45f;
    public float currScore;



    // public GameObject ballPrefab;
    // public Transform handTransform;
    // public bool isBallInHand;
    // public GameObject currentBall;
    // public float releaseForce = 10f;
    // public float spawnBallY = 3f;
    // public float spawnBallFrequency = 3f;
    // public float BallSlowdown = 1f;
    // public float SlowDownIntensity = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vrCamera = Camera.main.transform; // Assuming your VR camera is the main camera
        // InvokeRepeating("SpawnBall", 0f, spawnBallFrequency); // Spawn a ball every 10 seconds
        // BallSlowdown = 1f;

    }

    void Update()
    {
        if (isMovementEnabled)
        {
            Move();
            Jump();
        }
        ApplyCustomGravity();
        currScore = transform.position.z + offsetScore;

        if (transform.position.y < 0f)
        {
            PlayerPrefs.SetFloat(lastScoreKey, currScore);
            PlayerPrefs.Save();
            GameOverA();
        }
    }



    void Move()
    {
        // Get the forward direction of the VR camera
        Vector3 cameraForward = vrCamera.forward;
        cameraForward.y = 0f; // Keep the direction only in the XZ plane

        // Calculate the speed based on the forward direction
        speed = Mathf.Clamp(cameraForward.z, 0f, maxSpeed) * maxSpeed;// * BallSlowdown;

        // Set the velocity of the Rigidbody
        rb.velocity = cameraForward.normalized * speed;

        // Handle rotation towards the camera's forward direction
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationLerpSpeed);
    }

    void Jump()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        // Trigger jump on tap or mouse click if grounded

        if ((isGrounded && (Input.anyKeyDown && !Input.GetButtonDown("Cancel"))))
        {
            isJumping = true;
            currentJumpForce = jumpForce;
        }
        // else if ((isGrounded && (Input.anyKeyDown && !Input.GetButtonDown("Cancel"))))
        // {
        //     // ReleaseBall();
        // }

        // Apply jumping force gradually
        if (isJumping)
        {
            rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
            currentJumpForce -= Time.deltaTime * jumpForce; // Gradual decrease of force

            if (currentJumpForce <= 0f)
            {
                isJumping = false;
                currentJumpForce = 0f;
            }
        }
    }

    void ApplyCustomGravity()
    {
        // Apply custom gravity
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravity);
        }
    }
    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    // Add a method to disable movement
    public void DisableMovement()
    {
        isMovementEnabled = false;
    }
    void GameOverA()
    {
        // Load the next scene (you can replace this with the appropriate scene index or name)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SlowDown(float slowdown)
    {
        speed -= slowdown;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Grabbable"))
        {
            // Debug.Log("Ball Grabbed");
            // GrabBall(collision.gameObject);
        }
        else if (collision.collider.CompareTag("Monster"))
        {
            // Assuming you have a method in your Monster script to slow it down
            collision.collider.GetComponent<MonsterFollow>().SlowDown(5f);
        }
    }
    // private void SpawnBall()
    // {
    //     float randomZ = Random.Range(transform.position.z, transform.position.z + 150f);
    //     Vector3 spawnPosition = new Vector3(0f, spawnBallY, randomZ);
    //     Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    // }

    // private void GrabBall(GameObject ball)
    // {
    //     isBallInHand = true;
    //     currentBall = ball;

    //     // Set the grabbed ball as a child of the player
    //     currentBall.transform.SetParent(handTransform);
    //     currentBall.transform.position = handTransform.position;

    //     // Disable physics for the grabbed ball
    //     Rigidbody ballRigidbody = currentBall.GetComponent<Rigidbody>();
    //     ballRigidbody.useGravity = false;
    //     ballRigidbody.velocity = Vector3.zero;
    //     // ballRigidbody.isKinematic = true;
    //     // ballRigidbody.constraints = RigidbodyConstraints.FreezeAll;

    //     // Slow down the player
    //     BallSlowdown = SlowDownIntensity;
    // }


    // void ReleaseBall()
    // {
    //     Debug.Log("Ball Released");
    //     handTransform.DetachChildren();
    //     isBallInHand = false;
    //     Rigidbody ballRigidbody = currentBall.GetComponent<Rigidbody>();
    //     ballRigidbody.isKinematic = false;
    //     ballRigidbody.useGravity = true;
    //     ballRigidbody.constraints = RigidbodyConstraints.None;

    //     // Apply force to the ball in the opposite direction of the camera's forward vector
    //     Vector3 releaseDirection = Camera.main.transform.forward;
    //     ballRigidbody.AddForce(releaseDirection * releaseForce, ForceMode.Impulse);
    //     BallSlowdown = 1f;
    // }

}
