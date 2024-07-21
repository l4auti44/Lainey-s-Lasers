using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportsForTest : MonoBehaviour
{
    [SerializeField] private Transform[] teleports;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = teleports[0].position;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.position = teleports[1].position;
        }
    }
}
