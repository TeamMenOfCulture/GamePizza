using UnityEngine;

public class GVREmulator : MonoBehaviour
{
    public float rotationSpeed = 3.0f;
    private bool isRotating;

    void Update()
    {
        // Check if Ctrl key is pressed
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            // Check for mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotate the camera based on mouse input
            RotateCamera(mouseX, mouseY);
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
    }

    void RotateCamera(float mouseX, float mouseY)
    {
        // Calculate rotation angles based on mouse input
        float rotationX = mouseY * rotationSpeed;
        float rotationY = mouseX * rotationSpeed;

        // Apply rotation to the camera
        transform.Rotate(-rotationX, rotationY, 0);
    }

    void OnGUI()
    {
        // Display a message when Ctrl is held down and the mouse is moving
        if (isRotating)
        {
            GUI.Label(new Rect(10, 10, 200, 30), "GVR Emulator: Mouse Control");
        }
    }
}
