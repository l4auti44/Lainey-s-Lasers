using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationLoadeer : MonoBehaviour
{
    private PlayerCam sens;
    // Start is called before the first frame update
    void Start()
    {
        sens = Camera.main.GetComponent<PlayerCam>();
        if (!PlayerPrefs.HasKey("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", 20f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sens.sensX_mouse = PlayerPrefs.GetFloat("Sensitivity");
        sens.sensY_mouse = PlayerPrefs.GetFloat("Sensitivity");
        sens.sensX_cont = PlayerPrefs.GetFloat("Sensitivity") * 10f;
        sens.sensY_cont = PlayerPrefs.GetFloat("Sensitivity") * 10f;
    }
}
