using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 10f;
    public KeyCode joyStickKeyCode = KeyCode.Joystick1Button1;

    void Update()
    {
        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            MoveForward();
        }

        // Check if the specified joystick button is held down
        if (Input.GetKey(joyStickKeyCode))
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        transform.position = transform.position + Camera.main.transform.forward * moveSpeed * Time.deltaTime;
    }
}
