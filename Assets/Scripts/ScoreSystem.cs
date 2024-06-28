using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public TextMesh scoreText;
    public Transform player;

    public float highScore = 0f;
    private string highScoreKey = "HighScore";
    public float offsetScore = 45f;
    void Start()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetFloat(highScoreKey, 0f);
        UpdateScoreText();
    }

    void Update()
    {
        // Update the score based on the player's Z-axis position
        float currentZScore = player.position.z + offsetScore;

        // Update the high score if the current score is higher
        if (currentZScore > highScore)
        {
            highScore = currentZScore;
            PlayerPrefs.SetFloat(highScoreKey, highScore);
            PlayerPrefs.Save();
        }
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        // Update the score text on the TextMesh component
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.Round(player.position.z + offsetScore).ToString() +
                             "\nHigh Score: " + Mathf.Round(highScore).ToString();
        }
    }
}
