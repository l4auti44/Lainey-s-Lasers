//using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    GameObject highscoreText;
    private void Start()
    {
        highscoreText = GameObject.Find("HighScore");
        highscoreText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        
    }

    private void OnTriggerStay(Collider other)
    {
        GameController gameCont = GameObject.Find("GameController").GetComponent<GameController>();

        if (!PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))//FIRST TIME
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, gameCont.timer);
            highscoreText.SetActive(true);
            var ts = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name));
            var highscore = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            highscoreText.GetComponent<TextMeshProUGUI>().text = "New HighScore: " + highscore;
        }
        else
        {
            if (gameCont.timer < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name))//NEW HIGHSCORE
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, gameCont.timer);

                highscoreText.SetActive(true);
                var ts = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name));
                var highscore = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
                highscoreText.GetComponent<TextMeshProUGUI>().text = "New HighScore: " + highscore;
            }


            gameCont.Win();
        }

    }
}
