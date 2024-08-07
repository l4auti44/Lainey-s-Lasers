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
        else if(other.gameObject.layer != 8) //8 layer is "PickeableObject"
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
        else if (other.gameObject.layer != 8)
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
        else if (other.gameObject.layer != 8)
        {
            other.transform.SetParent(null);
        }
        
    }
}
