using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SaberLevelManagerINF : MonoBehaviour
{
    public int GameOverSceneIndex;
    // Start is called before the first frame update
    void Start()
    {

            PlayerPrefs.SetInt("SaberScore", 0);
        
            PlayerPrefs.SetInt("MissCount", 0);
        
    }
    void Update()
    {
        int missCount = PlayerPrefs.GetInt("MissCount");
        if (missCount >= 9999)
        {
            GameOverScene();
        }
    }

    public void GameOverScene()
    {
        SceneManager.LoadScene(GameOverSceneIndex);
    }

}
