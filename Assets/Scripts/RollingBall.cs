using UnityEngine;

public class RollingBall : MonoBehaviour
{
    public float forceMagnitude = 100f;
    public float destroyTime = 6f;
    private bool isColliding = false;

    public float colliderOffTime = 6f;

    void Start()
    {
        // Set a random collider off time within the destroy time
        Invoke("TurnOffCollider", colliderOffTime);

        // Destroy the ball after a certain time
        Destroy(gameObject, destroyTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        if (collision.gameObject.CompareTag("Player") && !isColliding)
        {
            Debug.Log("with Player");

            Vector3 direction = Vector3.right;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction * forceMagnitude, ForceMode.Acceleration);

            isColliding = true;
        }
    }

    void TurnOffCollider()
    {
        // Turn off the collider of this specific ball
        GetComponent<Collider>().enabled = false;
    }
}
