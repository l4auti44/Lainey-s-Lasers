using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    //[SerializeField] private Slider music;
    //[SerializeField] private Slider fov;
    [SerializeField] private Slider Sensitivity;
    //[SerializeField] private Slider soundEffect;
    //[SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI sensNum;
    //[SerializeField] private TextMeshProUGUI fovNum;
    //[SerializeField] private TextMeshProUGUI musicNum;
    //[SerializeField] private TextMeshProUGUI soundEffectNum;
    // Start is called before the first frame update
    [SerializeField] private Toggle showTimer;
    void Start()
    {
        /*
        if (!PlayerPrefs.HasKey("music"))
        {
            PlayerPrefs.SetFloat("music", 0.7f);
        }
        music.value = PlayerPrefs.GetFloat("music");
        if (!PlayerPrefs.HasKey("fov"))
        {
            PlayerPrefs.SetFloat("fov", 60f);
        }
        fov.value = PlayerPrefs.GetFloat("fov");
        */
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 20f);
        }
        Sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        /*
        if (!PlayerPrefs.HasKey("soundEffect"))
        {
            PlayerPrefs.SetFloat("soundEffect", 1f);
        }
        soundEffect.value = PlayerPrefs.GetFloat("soundEffect");
        */
        if (!PlayerPrefs.HasKey("showTimer"))
        {
            PlayerPrefs.SetInt("showTimer", 1);
        }
        else
        {
            if (PlayerPrefs.GetInt("showTimer") == 0)
            {
                showTimer.isOn = false;
            }
            else
            {
                showTimer.isOn = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerPrefs.SetFloat("music", music.value);
        //audioSource.volume = music.value;
        //PlayerPrefs.SetFloat("fov", fov.value);
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity.value);
        //PlayerPrefs.SetFloat("soundEffect", soundEffect.value);
        //musicNum.text = (PlayerPrefs.GetFloat("music") * 100).ToString("00");
        //soundEffectNum.text = (PlayerPrefs.GetFloat("soundEffect") * 100).ToString("00");
        //fovNum.text = PlayerPrefs.GetFloat("fov").ToString("00.0");
        sensNum.text = PlayerPrefs.GetFloat("Sensitivity").ToString("00.0");
        if (showTimer.isOn)
        {
            PlayerPrefs.SetInt("showTimer", 1);
        }
        else
        {
            PlayerPrefs.SetInt("showTimer", 0);
        }
    }
}
