using UnityEngine;

[RequireComponent(typeof(Target))]
public class BoxCatcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Triggerer"))
        {
            this.GetComponent<Target>().DoAction();
        }
    }
}
