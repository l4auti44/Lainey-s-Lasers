using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    [SerializeField] private Slider music;
    [SerializeField] private Slider fov;
    [SerializeField] private Slider sensibility;
    [SerializeField] private Slider soundEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI sensNum;
    [SerializeField] private TextMeshProUGUI fovNum;
    [SerializeField] private TextMeshProUGUI musicNum;
    [SerializeField] private TextMeshProUGUI soundEffectNum;
    // Start is called before the first frame update
    void Start()
    {
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
        if (!PlayerPrefs.HasKey("sensibility"))
        {
            PlayerPrefs.SetFloat("sensibility", 3f);
        }
        sensibility.value = PlayerPrefs.GetFloat("sensibility");
        if (!PlayerPrefs.HasKey("soundEffect"))
        {
            PlayerPrefs.SetFloat("soundEffect", 1f);
        }
        soundEffect.value = PlayerPrefs.GetFloat("soundEffect");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("music", music.value);
        audioSource.volume = music.value;
        PlayerPrefs.SetFloat("fov", fov.value);
        PlayerPrefs.SetFloat("sensibility", sensibility.value);
        PlayerPrefs.SetFloat("soundEffect", soundEffect.value);
        musicNum.text = (PlayerPrefs.GetFloat("music") * 100).ToString("00");
        soundEffectNum.text = (PlayerPrefs.GetFloat("soundEffect") * 100).ToString("00");
        fovNum.text = PlayerPrefs.GetFloat("fov").ToString("00.0");
        sensNum.text = PlayerPrefs.GetFloat("sensibility").ToString("00.0");
    }
}
