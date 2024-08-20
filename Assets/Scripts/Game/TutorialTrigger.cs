using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private int tutorialNumber;
   private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            EventManager.Game.OnTutorialTrigger(this, tutorialNumber);
        }
    }
}
