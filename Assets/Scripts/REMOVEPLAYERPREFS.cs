using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REMOVEPLAYERPREFS : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
    }
}
