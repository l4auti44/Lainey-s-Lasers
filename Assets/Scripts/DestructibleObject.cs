using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DestructibleObject : MonoBehaviour
{
    private Vector3 startPosition;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
    }

    public void ResetGameObject()
    {
        EventManager.Game.OnObjectDestroy.Invoke(this, transform.name);
        transform.position = startPosition;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
