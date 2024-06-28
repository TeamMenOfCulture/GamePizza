using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showHighScore : MonoBehaviour
{
    // Start is called before the first frame update
    public float highScore;
    private string highScoreKey = "HighScore";
    public TextMesh scoreText;

    void Start()
    {
        highScore = PlayerPrefs.GetFloat(highScoreKey, 0f);
        scoreText.text = "High Score: " + Mathf.Round(highScore).ToString();
    }

}
