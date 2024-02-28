using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
