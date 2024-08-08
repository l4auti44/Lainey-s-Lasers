using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    private GameObject pauseMenu;
    private GameObject resumeButton;
    [HideInInspector] public float timer;


    private GameObject player;
    private HealthSystem healthSyst;
    private bool winCondition = false;
    private TMP_Text generalTimer;
    private GameObject recordText;

    private void Awake()
    {
        resumeButton = GameObject.Find("ResumeButton");
        pauseMenu = GameObject.Find("PauseMenu");
        generalTimer = GameObject.Find("GeneralTimer").GetComponent<TMP_Text>();
        recordText = GameObject.Find("RecordText");
        recordText.SetActive(false);
        Cursor.visible = false;
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        healthSyst = player.GetComponent<HealthSystem>();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGeneralTimer();

        if (SceneController.isPaused)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


    }

    private void UpdateGeneralTimer()
    {
        timer += Time.deltaTime;
        var ts = TimeSpan.FromSeconds(timer);
        generalTimer.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    public void Die()
    {
        pauseMenu.transform.Find("ResumeButton").gameObject.SetActive(false);
        SceneController.TriggerPause();
    }





    private void Win(Component component)
    {
        if (healthSyst.playerHealth == 100f)
        {
            EventManager.Game.OnHitless.Invoke(this);
        }


        //TIME SCORE
        var ts = TimeSpan.FromSeconds(timer);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + " time"))
        {
            if (PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + " time") > timer)
            {
                recordText.SetActive(true);
                recordText.GetComponent<TMP_Text>().text = "NEW RECORD!!\n" + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + " time", timer);
            }
        }
        else
        {
            recordText.SetActive(true);
            recordText.GetComponent<TMP_Text>().text = "NEW RECORD!!\n" + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + " time", timer);
        }
        

        //Time Achievement
        if (timer <= 30f)
        {
            
        }
        winCondition = true;
        SceneController.TriggerPause();
    }

    private void OnEnable()
    {
        EventManager.Game.OnWin += Win;
    }

    private void OnDisable()
    {
        EventManager.Game.OnWin -= Win;
    }

    private void OnPause()
    {

        if (healthSyst.playerHealth > 0 && !winCondition)
        {
            SceneController.TriggerPause();
        }

    }

}
