using UnityEngine;
using EzySlice;

public class Block : MonoBehaviour
{
    public float speed = 5f;
    public Material crossSectionMaterial;

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

        // Ensure plane orientation matches the saber's slicing direction
        Vector3 planePosition = saber.position;
        Vector3 planeNormal = saber.right; // Adjust if necessary

        // Debug.Log("Plane Position: " + planePosition);
        // Debug.Log("Plane Normal: " + planeNormal);

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
