using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    private GameObject pauseMenu;
    static public bool isPaused = false;
    [HideInInspector] public float timer;


    private GameObject player;
    private HealthSystem healthSyst;

    private void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
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
        //GENERAL TIMER
        timer += Time.deltaTime;
        var ts = TimeSpan.FromSeconds(timer);
        //generalTimer.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
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

        if (Input.GetKeyDown(KeyCode.Escape) && healthSyst.playerHealth > 0)
        {
            SceneController.TriggerPause();
        }
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

        //Time Achievement
        if (timer <= 30f)
        {
            
        }
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



}
