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
    [SerializeField] private float throwBonusPower = 100f;
    [SerializeField] private float throwBonusExponent = 1.05f;
    private PlayerMovementAdvanced playerMovement;
    private Rigidbody playerRB;
    private Vector2 playerInput;

    private int previuosLayer;

    [Header("BEBUG")]
    [SerializeField] private bool DEBUG = false;
    private LayerMask ignorePlayerBodyLayer;

    private void Awake()
    {
        ignorePlayerBodyLayer = LayerMask.GetMask("IgnorePickUpRay");
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerMovement = playerRB.transform.GetComponent<PlayerMovementAdvanced>();
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
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange, ~ignorePlayerBodyLayer))
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
            playerInput = playerMovement.inputMove;
            float throwBonus = (throwBonusPower/playerMovement.walkSpeed) * playerRB.velocity.magnitude; //value 7 is the player sprint speed
            throwBonus = Mathf.Pow(throwBonus, throwBonusExponent);
            Vector3 force = transform.forward * throwPower;
            Vector3 bonusForce = transform.forward * (throwBonus * playerInput.y) + transform.right * (throwBonus * playerInput.x);
            heldObjRB.AddForce(force + bonusForce, ForceMode.Force);
            if (DEBUG) Debug.Log("Bonus force: " + (playerInput * throwBonus).ToString());  
            heldObj = null;

        }
    }
    private void CheckIfObjectDestroy(Component component, string objectName)
    {
        if (heldObj)
        {
            if (heldObj.name == objectName)
            {
                DropObject();
            }
        }
    }

    private void OnEnable()
    {
        EventManager.Game.OnObjectDestroy += CheckIfObjectDestroy;
    }
    private void OnDisable()
    {
        EventManager.Game.OnObjectDestroy -= CheckIfObjectDestroy;
    }
}
