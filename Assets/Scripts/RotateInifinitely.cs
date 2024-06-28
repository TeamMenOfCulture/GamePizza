using UnityEngine;

public class RotateInifinitely : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()
    {
        // Rotate the GameObject around its Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeed / 2 * Time.deltaTime);
    }
    public void ChangeSpin()
    {
        // Rotate the GameObject around its Y-axis
        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, -rotationSpeed / 2 * Time.deltaTime);
    }
}
