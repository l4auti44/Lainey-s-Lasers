using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovementAdvanced pm;

    [Header("Sliding")]


    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    private bool slidingInput = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (slidingInput && !pm.sliding && !pm.crouching)
            StartSlide();

        if (!slidingInput && (pm.sliding || pm.crouching) && !pm.somethingAbove)
            StopSlide();
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        

        if (rb.velocity.magnitude <= 0 && pm.state == PlayerMovementAdvanced.MovementState.idle)
        {
            pm.crouching = true;
            pm.sliding = false;
        }
        else
        {
            pm.sliding = true;
            pm.crouching = false;
        }

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        
        
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // sliding normal
        if(!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized, ForceMode.Force);

            //slideTimer -= Time.deltaTime;
        }

        // sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection), ForceMode.Force);
        }

        //if (slideTimer <= 0 && !pm.somethingAbove)
            //StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;
        pm.crouching = false;
        pm.currentTimeCrouchBuff = 0;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }

    private void OnSliding()
    {
        if (!SceneController.isPaused)
            //press and release
            slidingInput = !slidingInput;
    }
}
