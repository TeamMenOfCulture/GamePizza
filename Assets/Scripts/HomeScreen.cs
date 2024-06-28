using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HomeScreen : MonoBehaviour
{
    public GameObject startGameButton;
    public GameObject exitButton;
    public Material materialA;
    public Material materialB;
    public Text countdownText; // UI Text element to display the countdown

    private Renderer startGameRenderer;
    private Renderer exitRenderer;
    private Coroutine countdownCoroutine;

    void Start()
    {
        // Get the renderers of the buttons
        startGameRenderer = startGameButton.GetComponent<Renderer>();
        exitRenderer = exitButton.GetComponent<Renderer>();

        // Set the initial materials
        startGameRenderer.material = materialA;
        exitRenderer.material = materialA;

        // Hide the countdown text initially
        countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Get the ray from the camera to the screen middle
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // RaycastHit variable to store information about the hit
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("StartGame"))
            {
                // Change material on hover
                startGameRenderer.material = materialB;

                if (countdownCoroutine == null)
                {
                    countdownCoroutine = StartCoroutine(CountdownToAction(3, () => SceneManager.LoadScene(1)));
                }
            }
            else if (hit.collider.CompareTag("Exit"))
            {
                // Change material on hover
                exitRenderer.material = materialB;

                if (countdownCoroutine == null)
                {
                    countdownCoroutine = StartCoroutine(CountdownToAction(3, () => Application.Quit()));
                }
            }
            else
            {
                // Revert materials on hover end and stop countdown
                ResetHover();
            }
        }
        else
        {
            // Revert materials if no object was hit and stop countdown
            ResetHover();
        }
    }

    private IEnumerator CountdownToAction(float seconds, System.Action action)
    {
        countdownText.gameObject.SetActive(true);

        float counter = seconds;
        while (counter > 0)
        {
            countdownText.text = counter.ToString("F1");
            yield return new WaitForSeconds(0.1f);
            counter -= 0.1f;
        }

        countdownText.gameObject.SetActive(false);
        action.Invoke();
    }

    private void ResetHover()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
        startGameRenderer.material = materialA;
        exitRenderer.material = materialA;
        countdownText.gameObject.SetActive(false);
    }
}
