using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAtoB : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed;
    private Vector3 targetPosition;
    // Update is called once per frame
    private void Start()
    {
        targetPosition = pointA.position;
    }
    void FixedUpdate()
    {
       transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }

        
    }
}
