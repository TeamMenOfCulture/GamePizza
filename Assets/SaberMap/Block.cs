using UnityEngine;
using EzySlice;

public class Block : MonoBehaviour
{
    public float speed = 5f;
    public Material crossSectionMaterial;
    public GameObject explosionPrefab; // Reference to the explosion prefab

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
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

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Slicing Failed");
            TriggerExplosion();
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
            // Instantiate the explosion prefab at the block's position and rotation
            GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, transform.rotation);

            // Play all particle systems within the explosion prefab
            ParticleSystem[] particleSystems = explosionInstance.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }

            // Destroy the explosion instance after the particles have finished playing
            Destroy(explosionInstance, 2f); // Adjust the duration based on the lifetime of the particles
        }
    }
}
