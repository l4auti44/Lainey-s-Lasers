using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : Target
{
    [SerializeField] private GameObject door;
    
    override
    public void DoAction()
    {
        door.GetComponent<MoveToA>().openDoor = true;
    }

    override
    public void Undo()
    {
        door.GetComponent<MoveToA>().openDoor = false;
    }
}
