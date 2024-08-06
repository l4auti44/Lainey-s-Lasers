using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverItems : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DestructibleObject>())
        {
            collision.gameObject.GetComponent<DestructibleObject>().ResetGameObject();
        }
    }
}
