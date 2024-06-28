using UnityEngine;

public class ButtonPressController : MonoBehaviour
{
    public TextMesh textMesh;
    public bool joystick1Pressed = false;
    public bool joystick2Pressed = false;

    // New boolean to track the state of the 'E' key
    public bool isEPressed = false;

    void Update()
    {
        if (Input.anyKeyDown || true)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    textMesh.text = "Button Pressed: " + keyCode;
                }
            }
        }
    }
}
