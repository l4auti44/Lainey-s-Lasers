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
    //private Slider sensibility, fov, music, soundEffect;

    private GameObject player;
    private HealthSystem healthSyst;



    private void Start()
    {
        player = GameObject.Find("Player");
        healthSyst = player.GetComponent<HealthSystem>();
        /*
        #region FindObjects
        generalTimer = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        
       // virtualCamera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        audioSource = GameObject.Find("Audio").GetComponent<AudioSource>();
        playerAudioSource = player.GetComponent<AudioSource>();

        pauseMenu = GameObject.Find("PauseMenu");
        music = pauseMenu.transform.Find("Music").GetComponent<Slider>();
        fov = pauseMenu.transform.Find("Fov").GetComponent<Slider>();
        sensibility = pauseMenu.transform.Find("Sensibility").GetComponent<Slider>();
        soundEffect = pauseMenu.transform.Find("SoundEffect").GetComponent<Slider>();
        sensNum = pauseMenu.transform.Find("Sensibility Number").GetComponent<TextMeshProUGUI>();
        fovNum = pauseMenu.transform.Find("Fov Number").GetComponent<TextMeshProUGUI>();
        musicNum = pauseMenu.transform.Find("Music Number").GetComponent<TextMeshProUGUI>();
        soundEffectNum = pauseMenu.transform.Find("SoundEffect Number").GetComponent <TextMeshProUGUI>();

        #endregion

        #region PlayerPrefs
        if (!PlayerPrefs.HasKey("sensibility"))
        {
            //PlayerPrefs.SetFloat("sensibility", player.GetComponent<FirstPersonController>().RotationSpeed);
        }
        else
        {
            //player.GetComponent<FirstPersonController>().RotationSpeed = PlayerPrefs.GetFloat("sensibility");
        }
        //sensibility.value = player.GetComponent<FirstPersonController>().RotationSpeed;
        sensNum.text = sensibility.value.ToString("00.0");
        
        if (!PlayerPrefs.HasKey("fov"))
        {
            //PlayerPrefs.SetFloat("fov", virtualCamera.m_Lens.FieldOfView);
        }
        else
        {
            //virtualCamera.m_Lens.FieldOfView = PlayerPrefs.GetFloat("fov");
        }
       // fov.value = virtualCamera.m_Lens.FieldOfView;
        fovNum.text = fov.value.ToString("00.0");

        if (!PlayerPrefs.HasKey("soundEffect"))
        {
            PlayerPrefs.SetFloat("soundEffect", 1f);
        }
        soundEffect.value = PlayerPrefs.GetFloat("soundEffect");
        soundEffectNum.text = (soundEffect.value * 100).ToString("00");
        audioSource.volume = PlayerPrefs.GetFloat("music");
        
        music.value = audioSource.volume;
        musicNum.text = (music.value * 100).ToString("00");

        #endregion
        */
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        //GENERAL TIMER
        timer += Time.deltaTime;
        var ts = TimeSpan.FromSeconds(timer);
        //generalTimer.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && healthSyst.playerHealth > 0)
        {
            TriggerPause();
        }
    }

    public void TriggerPause()
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

    public void Die()
    {
        //TriggerPause();
    }





    public void Win()
    {
        if (healthSyst.playerHealth == 100f)
        {
            //HITLESS
        }

        //Time Achievement
        if (timer <= 30f)
        {
            
        }
        TriggerPause();
    }


}
