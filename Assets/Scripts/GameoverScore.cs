using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverScore : MonoBehaviour
{
    // Start is called before the first frame update
    public float highScore;
    private string latestScore = "LatestScore";
    public TextMesh scoreText;

    void Start()
    {
        highScore = PlayerPrefs.GetFloat(latestScore, 0f);
        scoreText.text = "Score: " + Mathf.Round(highScore).ToString();
    }
}
