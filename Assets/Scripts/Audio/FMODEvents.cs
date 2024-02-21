using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }

    //EXAMPLE
    [field: Header("Coin SFX")]
    [field: SerializeField] public EventReference coinCollected { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("\"Found more than one FMOD Events instance in the scene.");
        }
    }
}
