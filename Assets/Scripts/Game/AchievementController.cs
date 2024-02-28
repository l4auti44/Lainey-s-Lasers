using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    [SerializeField] private Texture activeAchievement;
    [SerializeField] private Texture notActiveAchievement;
    private RawImage time1, hitless1;
    private RawImage time2, hitless2;
    private RawImage time3, hitless3;
    // Start is called before the first frame update
    void Start()
    {
        time1 = GameObject.Find("Time1").GetComponent<RawImage>();
        time2 = GameObject.Find("Time2").GetComponent<RawImage>();
        time3 = GameObject.Find("Time3").GetComponent<RawImage>();
        hitless1 = GameObject.Find("Hitless1").GetComponent<RawImage>();
        hitless2 = GameObject.Find("Hitless2").GetComponent<RawImage>();
        hitless3 = GameObject.Find("Hitless3").GetComponent<RawImage>();

        SetUpPlayerPrefs();
        LoadAchievements();
        
    }

    private void SetUpPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("Level 1 time"))
        {
            PlayerPrefs.SetInt("Level 1 time", 0);
        }
        if (!PlayerPrefs.HasKey("Level 2 time"))
        {
            PlayerPrefs.SetInt("Level 2 time", 0);
        }
        if (!PlayerPrefs.HasKey("Level 3 time"))
        {
            PlayerPrefs.SetInt("Level 3 time", 0);
        }
        if (!PlayerPrefs.HasKey("Level 1 hitless"))
        {
            PlayerPrefs.SetInt("Level 1 hitless", 0);
        }
        if (!PlayerPrefs.HasKey("Level 2 hitless"))
        {
            PlayerPrefs.SetInt("Level 2 hitless", 0);
        }
        if (!PlayerPrefs.HasKey("Level 3 hitless"))
        {
            PlayerPrefs.SetInt("Level 3 hitless", 0);
        }

    }
    
    private void LoadAchievements()
    {
        if (PlayerPrefs.GetInt("Level 1 time") == 0)
        {
            time1.texture = notActiveAchievement;
        }
        else
        {
            time1.texture = activeAchievement;
        }
        if (PlayerPrefs.GetInt("Level 2 time") == 0)
        {
            time2.texture = notActiveAchievement;
        }
        else
        {
            time2.texture = activeAchievement;
        }
        if (PlayerPrefs.GetInt("Level 3 time") == 0)
        {
            time3.texture = notActiveAchievement;
        }
        else
        {
            time3.texture = activeAchievement;
        }
        if (PlayerPrefs.GetInt("Level 1 hitless") == 0)
        {
            hitless1.texture = notActiveAchievement;
        }
        else
        {
            hitless1.texture = activeAchievement;
        }
        if (PlayerPrefs.GetInt("Level 2 hitless") == 0)
        {
            hitless2.texture = notActiveAchievement;
        }
        else
        {
            hitless2.texture = activeAchievement;
        }
        if (PlayerPrefs.GetInt("Level 3 hitless") == 0)
        {
            hitless3.texture = notActiveAchievement;
        }
        else
        {
            hitless3.texture = activeAchievement;
        }
    }
}
