using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    [Header("Throw objects")]
    [SerializeField] private float throwPower = 500f;
    private Rigidbody playerRB;

    private int previuosLayer;

    private void Awake()
    {
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {

        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = holdArea.position - heldObj.transform.position;
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            previuosLayer = pickObj.layer;
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            
            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
            heldObj.layer = 8;
        }
    }

    void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObj.layer = previuosLayer;
        heldObj.transform.parent = null;
        heldObj = null;
        
    }

    private void OnPickUpPutDown()
    {
        if (heldObj == null)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.green, 2f);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
            {
                PickupObject(hit.transform.gameObject);
            }
        }
        else
        {
            DropObject();
        }
    }

    private void OnThrow()
    {
        
        if (heldObj != null)
        {
            heldObjRB.useGravity = true;
            heldObjRB.drag = 1;
            heldObjRB.constraints = RigidbodyConstraints.None;
            heldObj.layer = previuosLayer;
            heldObj.transform.parent = null;
            if (playerRB.velocity.magnitude > 1)
            {
                heldObjRB.AddForce(this.transform.forward * throwPower * (playerRB.velocity.magnitude/4), ForceMode.Force);
            }
            else
            {
                heldObjRB.AddForce(this.transform.forward * throwPower, ForceMode.Force);
            }
            
            heldObj = null;

        }
    }
}
