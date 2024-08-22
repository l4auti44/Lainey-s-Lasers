using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckHitlessLevels : MonoBehaviour
{
    [SerializeField] private RawImage[] hitlessLevelImages;
    [SerializeField] private RawImage[] UnderTimeLevelImages;
    [SerializeField] private Texture noAchievement;
    [SerializeField] private Texture Achievement;
    private void Start()
    {
        int count = 1;
        foreach (var lvImage in hitlessLevelImages)
        {
            if (PlayerPrefs.HasKey("Level " + count.ToString() + " Hitless"))
            {
                lvImage.texture = Achievement;
            }
            else
            {
                lvImage.texture = noAchievement;
            }


            count++;
        }
        int count2 = 1;
        foreach(var underTime in UnderTimeLevelImages)
        {
            if (PlayerPrefs.HasKey("Level " +  count2.ToString() + " time"))
            {
                if (PlayerPrefs.GetFloat("Level " + count2.ToString() + " time") <= 35f)
                {
                    underTime.texture = Achievement;
                }
                else
                {
                    underTime.texture = noAchievement;
                }
            }
        }
    }

}
