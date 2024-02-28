
//using Cinemachine;
//using StarterAssets;
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
    private Slider sensibility, fov, music, soundEffect;

    [HideInInspector] public GameObject player;
    //private CinemachineVirtualCamera virtualCamera;
    private AudioSource audioSource;
    private TextMeshProUGUI sensNum, fovNum, musicNum, soundEffectNum,generalTimer;
    private HealthSystem healthSyst;
    private AudioSource playerAudioSource;
    // Start is called before the first frame update

    private void Start()
    {
        #region FindObjects
        generalTimer = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        player = GameObject.Find("Player");
        healthSyst = player.GetComponent<HealthSystem>();
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
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageSliders();
        //GENERAL TIMER
        timer += Time.deltaTime;
        var ts = TimeSpan.FromSeconds(timer);
        generalTimer.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);


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


    private void ManageSliders()
    {

        //player.GetComponent<FirstPersonController>().RotationSpeed = sensibility.value;
        //virtualCamera.m_Lens.FieldOfView = fov.value;
        audioSource.volume = music.value;
        playerAudioSource.volume = soundEffect.value;
        //PlayerPrefs.SetFloat("sensibility", player.GetComponent<FirstPersonController>().RotationSpeed);
       //PlayerPrefs.SetFloat("fov", virtualCamera.m_Lens.FieldOfView = fov.value);
        PlayerPrefs.SetFloat("music", audioSource.volume);
        PlayerPrefs.SetFloat("soundEffect", playerAudioSource.volume);
        fovNum.text = fov.value.ToString("00.0");
        sensNum.text = sensibility.value.ToString("00.0");
        musicNum.text = (music.value * 100).ToString("00");
        soundEffectNum.text = (playerAudioSource.volume * 100).ToString("00");
    }



    public void TriggerPause()
    {
        pauseMenu.transform.GetChild(2).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "GAME PAUSED";
        if (Time.timeScale == 0)
        {
            audioSource.Play();
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            audioSource.Pause(); 
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Die()
    {
        TriggerPause();
        if (pauseMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>())
        {
            pauseMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "YOU DIED";
        }
        pauseMenu.transform.GetChild(2).gameObject.SetActive(false);
    }





    public void Win()
    {
        if (healthSyst.playerHealth == 100f)
        {
            //HITLESS
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + " hitless", 1);
           // player.GetComponent<FirstPersonController>().LaineyEnable(5);
            //player.GetComponent<FirstPersonController>().enabled = false;

        }
        //Time Achievement
        if (timer <= 30f)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + " time", 1);
        }
        TriggerPause();
        pauseMenu.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "YOU WIN!";
        pauseMenu.transform.GetChild(2).gameObject.SetActive(false);
    }


}
