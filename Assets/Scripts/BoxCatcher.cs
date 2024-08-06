using UnityEngine;

[RequireComponent(typeof(Target))]
public class BoxCatcher : MonoBehaviour
{
    private int objectsAmount = 0;
    private void OnTriggerEnter(Collider other)
    {
        objectsAmount++;

        this.GetComponent<Target>().DoAction();

    }
    private void OnTriggerExit(Collider other)
    {
        objectsAmount--;
        if (objectsAmount == 0)
        {
            this.GetComponent<Target>().Undo();
        }
    }


}
