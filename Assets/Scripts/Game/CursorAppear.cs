using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAppear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
