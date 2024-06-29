using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Add this for TextMeshPro

public class HomeScreen : MonoBehaviour
{
    public GameObject startGameButton;
    public GameObject exitButton;
    public int button1Index;
    public int button2Index;

    public Material materialA;
    public Material materialB;
    public TextMesh cntdwntxt; // Use TextMeshPro for 3D text
    
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
        cntdwntxt.gameObject.SetActive(false);
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
                    countdownCoroutine = StartCoroutine(CountdownToAction(3, () => SceneManager.LoadScene(button1Index)));
                }
            }
            else if (hit.collider.CompareTag("Exit"))
            {
                // Change material on hover
                exitRenderer.material = materialB;

                if (countdownCoroutine == null)
                {
                    countdownCoroutine = StartCoroutine(CountdownToAction(3, () =>
                    {
                        if (SceneManager.GetActiveScene().buildIndex == 0)
                        {
                            Application.Quit();
                        }
                        else
                        {
                            SceneManager.LoadScene(button2Index);
                        }
                    }));
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
        cntdwntxt.gameObject.SetActive(true);

        float counter = seconds;
        while (counter > 0)
        {
            cntdwntxt.text = counter.ToString("F1");
            yield return new WaitForSeconds(0.1f);
            counter -= 0.1f;
        }

        cntdwntxt.gameObject.SetActive(false);
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
        cntdwntxt.gameObject.SetActive(false);
    }
}
