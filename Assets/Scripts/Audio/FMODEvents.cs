using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Unity.Collections.LowLevel.Unsafe;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Connect Target and Laser SFX")]
    [field: SerializeField] public EventReference connectTargetLaser { get; private set; }

    [field: Header("Disconnect Target and Laser SFX")]
    [field: SerializeField] public EventReference disconnectTargetLaser { get; private set; }
    
    [field: Header("Button Select SFX")]
    [field: SerializeField] public EventReference buttonSelect { get; private set; }

    
    
    [field: Header("-Collect SFX")]
    [field: SerializeField] public EventReference collect { get; private set; }


    [field: Header("-Level Complete SFX")]
    [field: SerializeField] public EventReference levelComplete { get; private set; }


    [field: Header("-Player Death SFX")]
    [field: SerializeField] public EventReference playerDeath { get; private set; }


    [field: Header("-Taking Damage SFX")]
    [field: SerializeField] public EventReference takingDamage { get; private set; }




    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("\"Found more than one FMOD Events instance in the scene.");
        }
        else
        {
            instance = this;
        }
    }
}
