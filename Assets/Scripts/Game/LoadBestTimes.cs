using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadBestTimes : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level1;
    [SerializeField] private TextMeshProUGUI level2;
    [SerializeField] private TextMeshProUGUI level3;

    private void Start()
    {
        float text1 = PlayerPrefs.GetFloat("Level 1");
        if (PlayerPrefs.HasKey("Level 1"))
        {
            var ts1 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 1"));
            level1.text = string.Format("{0:00}:{1:00}", ts1.Minutes, ts1.Seconds);
        }
        if (PlayerPrefs.HasKey("Level 2"))
        {
            var ts2 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 2"));
            level2.text = string.Format("{0:00}:{1:00}", ts2.Minutes, ts2.Seconds);
        }
        if (PlayerPrefs.HasKey("Level 3"))
        {
            var ts3 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 3"));
            level3.text = string.Format("{0:00}:{1:00}", ts3.Minutes, ts3.Seconds);
        }
        

    }
}
