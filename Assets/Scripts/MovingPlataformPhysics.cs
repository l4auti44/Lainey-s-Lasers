using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataformPhysics : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.transform.parent.SetParent(transform);
        }
        else
        {
            other.transform.SetParent(transform);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.transform.parent.SetParent(transform);
        }
        else
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.transform.parent.SetParent(null);
        }
        else
        {
            other.transform.SetParent(null);
        }
        
    }
}
