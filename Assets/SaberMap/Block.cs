using UnityEngine;
using EzySlice;

public class Block : MonoBehaviour
{
    public float speed = 5f;
    public Material crossSectionMaterial; // Material for the cross-section of the sliced hulls

    void Update()
    {
        // Move the block
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
        // Use EZY Slice framework to slice the block
        Debug.Log("Slicing Initiated");
        EzySlice.Plane plane = new EzySlice.Plane();
        plane.Compute(saber.position, saber.up);

        // Visualize the slicing plane
        Debug.DrawRay(saber.position, saber.up * 5, Color.red, 2.0f);

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
}
