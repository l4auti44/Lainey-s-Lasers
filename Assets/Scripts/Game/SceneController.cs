using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    public static bool isPaused = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public static void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

    public static void SceneLoader(string name)
    {
        if (isPaused) TriggerPause();
        SceneManager.LoadScene(name);
    }

    public static void RestartCurrentScene()
    {
        if (isPaused) TriggerPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void TriggerPause()
    {
        if (Time.timeScale == 0)
        {
            isPaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
        }
    }


}