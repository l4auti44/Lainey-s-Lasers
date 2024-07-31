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
            if (teleports[0] != null)
            {
                transform.position = teleports[0].position;
            }
            else
            {
                Debug.Log("There is no teleport (transform.position) assigned in TeleportForTest.cs in Player GameObject");
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (teleports[1] != null)
            {
                transform.position = teleports[1].position;
            }
            else
            {
                Debug.Log("There is no teleport (transform.position) assigned in TeleportForTest.cs in Player GameObject");
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (teleports[2] != null)
            {
                transform.position = teleports[2].position;
            }
            else
            {
                Debug.Log("There is no teleport (transform.position) assigned in TeleportForTest.cs in Player GameObject");
            }

        }
    }
}
