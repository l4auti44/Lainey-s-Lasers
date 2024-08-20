using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckHitlessLevels : MonoBehaviour
{
    [SerializeField] private RawImage[] hitlessLevelImages;
    [SerializeField] private Texture noHitlessImage;
    [SerializeField] private Texture HitlessImage;
    private void Start()
    {
        int count = 1;
        foreach (var lvImage in hitlessLevelImages)
        {
            if (PlayerPrefs.HasKey("Level " + count.ToString() + " Hitless"))
            {
                lvImage.texture = HitlessImage;
            }
            else
            {
                lvImage.texture = noHitlessImage;
            }


            count++;
        }
    }
}
