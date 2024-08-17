using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSlideTrigerer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.transform.parent.GetComponent<Sliding>().forceSlide = true;
        }
    }
}
