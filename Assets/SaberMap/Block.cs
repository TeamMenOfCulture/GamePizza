using UnityEngine;
using EzySlice;
using UnityEngine.SceneManagement;

public class Block : MonoBehaviour
{
    public float speed = 5f;
    public Material crossSectionMaterial;
    public GameObject explosionPrefab; // Reference to the explosion prefab

    private TextMesh scoreTextMesh;
    private TextMesh missCountTextMesh;
    public Material missMaterial; // Material for missed blocks

    private void Start()
    {
        // Initialize PlayerPrefs for score and miss count if not already set
        if (!PlayerPrefs.HasKey("SaberScore"))
        {
            PlayerPrefs.SetInt("SaberScore", 0);
        }
        if (!PlayerPrefs.HasKey("MissCount"))
        {
            PlayerPrefs.SetInt("MissCount", 0);
        }

        // Get the TextMesh components from the named GameObjects
        scoreTextMesh = GameObject.Find("ScoreText").GetComponent<TextMesh>();
        missCountTextMesh = GameObject.Find("HitText").GetComponent<TextMesh>();

        UpdateUI();
    }

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Check if the block has moved past the threshold
        if (transform.position.z < -40f)
        {
            DisableBlock();
        }
        if (transform.position.z < -40.5f)
        {
            MissBlock();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Saber"))
        {
            Debug.Log("Saber Hit Detected");
            Slice(other.transform);
        }
    }

    private void Slice(Transform saber)
    {
        Debug.Log("Slicing Initiated");

        Vector3 planePosition = saber.position;
        Vector3 planeNormal = saber.right;

        Debug.DrawRay(planePosition, planeNormal * 5, Color.red, 2.0f);

        EzySlice.Plane plane = new EzySlice.Plane();
        plane.Compute(planePosition, planeNormal);

        SlicedHull slicedHull = gameObject.Slice(plane, crossSectionMaterial);

        if (slicedHull != null)
        {
            Debug.Log("Slicing Successful");
            GameObject upperHull = slicedHull.CreateUpperHull(gameObject, crossSectionMaterial);
            GameObject lowerHull = slicedHull.CreateLowerHull(gameObject, crossSectionMaterial);

            AddComponentsToSlice(upperHull);
            AddComponentsToSlice(lowerHull);

            // Update score
            UpdateScore(5);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Slicing Failed");
            TriggerExplosion();
            UpdateScore(1);
            Destroy(gameObject);
        }
    }

    private void AddComponentsToSlice(GameObject slice)
    {
        if (slice != null)
        {
            MeshCollider meshCollider = slice.AddComponent<MeshCollider>();
            meshCollider.convex = true;
            Rigidbody rb = slice.AddComponent<Rigidbody>();
            rb.AddExplosionForce(1000f, transform.position, 5f);
            Destroy(slice, 2f);
        }
    }

    private void TriggerExplosion()
    {
        if (explosionPrefab != null)
        {
            GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, transform.rotation);

            ParticleSystem[] particleSystems = explosionInstance.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }

            Destroy(explosionInstance, 2f);
        }
    }

    private void MissBlock()
    {
        int missCount = PlayerPrefs.GetInt("MissCount") + 1;
        PlayerPrefs.SetInt("MissCount", missCount);
        Debug.Log("Miss : " + missCount);

        if (missCount >= 5)
        {
            Debug.Log("Game Over");
        }

        UpdateUI();
    }

    private void UpdateScore(int points)
    {
        int currentScore = PlayerPrefs.GetInt("SaberScore") + points;
        PlayerPrefs.SetInt("SaberScore", currentScore);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreTextMesh != null)
        {
            scoreTextMesh.text = "Score: " + PlayerPrefs.GetInt("SaberScore");
        }

        if (missCountTextMesh != null)
        {
            missCountTextMesh.text = "Miss : " + PlayerPrefs.GetInt("MissCount");
        }
    }
    private void DisableBlock()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = missMaterial;
        }

        Destroy(gameObject, 2f); // Destroy the block after 2 seconds
    }
}
