using UnityEngine;

public class testRotate : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public Transform playertransform;
    void Update()
    {
        // Get the horizontal input (A and D keys)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the rotation amount based on the input and speed
        float rotationAmounth = horizontalInput * rotationSpeed * Time.deltaTime;
        float rotationAmountv = verticalInput * rotationSpeed * Time.deltaTime;

        // Rotate the object around the Y-axis
        playertransform.Rotate(Vector3.up, rotationAmounth);
        playertransform.Rotate(Vector3.left, rotationAmountv);
    }
}
