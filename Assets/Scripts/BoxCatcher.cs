using UnityEngine;

[RequireComponent(typeof(Target))]
public class BoxCatcher : MonoBehaviour
{
    private int objectsAmount = 0;
    private void OnTriggerEnter(Collider other)
    {
        objectsAmount++;
        if (other.CompareTag("Triggerer"))
        {
            this.GetComponent<Target>().DoAction();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        objectsAmount--;
        if (other.CompareTag("Triggerer") && objectsAmount == 0)
        {
            this.GetComponent<Target>().Undo();
        }
    }


}
