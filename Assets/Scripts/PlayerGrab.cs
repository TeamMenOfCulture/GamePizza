using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public GameObject myHand;
    public GameObject ballPrefab;
    public Transform playerTransform;
    private bool inHand = false;
    private Vector3 ballPos;
    public float grabOffset = 0.1f;
    public float releaseForce = 5f;
    private Rigidbody ballRigidbody;
    private float nextSpawnTime = 0f;
    public float spawnInterval = 10f;
    public float playerSpeedReduction = 3f;

    void Start()
    {
        ballPos = myHand.transform.position;
        ballRigidbody = ballPrefab.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (TryGrabObject())
            {
                GrabObject();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (inHand)
            {
                ReleaseObject();
            }
        }

        // Spawn ball every 10 seconds
        if (Time.time >= nextSpawnTime)
        {
            SpawnBall();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnBall()
    {
        float randomZ = Random.Range(playerTransform.position.z, playerTransform.position.z + 150f);
        Vector3 spawnPosition = new Vector3(0f, 10f, randomZ);
        Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }

    bool TryGrabObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.CompareTag("Grabbable");
        }

        return false;
    }

    void GrabObject()
    {
        ballPos = myHand.transform.position;
        GameObject grabbableObject = GetGrabbableObject();

        if (grabbableObject != null)
        {
            grabbableObject.transform.SetParent(myHand.transform);
            grabbableObject.transform.localPosition = new Vector3(0f, -grabOffset, 0f);
            ballRigidbody.useGravity = false;
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
            inHand = true;

            // Slow down the player
            SlowDown(playerSpeedReduction);
        }
    }

    void ReleaseObject()
    {
        myHand.transform.DetachChildren();
        inHand = false;
        ballRigidbody.useGravity = true;
        ballRigidbody.constraints = RigidbodyConstraints.None;

        // Apply force to the ball in the opposite direction of the camera's forward vector
        Vector3 releaseDirection = Camera.main.transform.forward;
        ballRigidbody.AddForce(releaseDirection * releaseForce, ForceMode.Impulse);
    }

    GameObject GetGrabbableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    public void SlowDown(float monsterSlowdown)
    {
        // Adjust player's moveSpeed or equivalent variable
        // e.g., moveSpeed -= monsterSlowdown;
    }
}
