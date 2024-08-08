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
    [SerializeField] private TextMeshProUGUI level4;
    [SerializeField] private TextMeshProUGUI level5;
    [SerializeField] private TextMeshProUGUI level6;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Level 1 time"))
        {
            var ts1 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 1 time"));
            level1.text = string.Format("{0:00}:{1:00}", ts1.Minutes, ts1.Seconds);
        }
        if (PlayerPrefs.HasKey("Level 2 time"))
        {
            var ts2 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 2 time"));
            level2.text = string.Format("{0:00}:{1:00}", ts2.Minutes, ts2.Seconds);
        }
        if (PlayerPrefs.HasKey("Level 3 time"))
        {
            var ts3 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 3 time"));
            level3.text = string.Format("{0:00}:{1:00}", ts3.Minutes, ts3.Seconds);
        }
        if (PlayerPrefs.HasKey("Level 4 time"))
        {
            var ts4 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 4 time"));
            level4.text = string.Format("{0:00}:{1:00}", ts4.Minutes, ts4.Seconds);
        }
        if (PlayerPrefs.HasKey("Level 5 time"))
        {
            var ts5 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 5 time"));
            level5.text = string.Format("{0:00}:{1:00}", ts5.Minutes, ts5.Seconds);
        }
        if (PlayerPrefs.HasKey("Level 6 time"))
        {
            var ts6 = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level 6 time"));
            level6.text = string.Format("{0:00}:{1:00}", ts6.Minutes, ts6.Seconds);
        }


    }
}
