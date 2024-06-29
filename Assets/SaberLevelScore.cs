using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberLevelScore : MonoBehaviour
{
    // Start is called before the first frame update
    public float highScore;
    public TextMesh scoreText;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("SaberScore");
        scoreText.text = "Score: " + highScore.ToString();
    }
}
