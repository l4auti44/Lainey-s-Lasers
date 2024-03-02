using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToA : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    private Vector3 firstPosition;
    [SerializeField] private float speed;
    private Vector3 targetPosition;

    [HideInInspector] public bool openDoor = false;
    // Update is called once per frame
    private void Start()
    {
        firstPosition = transform.position;
        targetPosition = pointA.position;
    }
    void Update()
    {
        if (openDoor)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, firstPosition, speed * Time.deltaTime);
        }
        
    }
}
