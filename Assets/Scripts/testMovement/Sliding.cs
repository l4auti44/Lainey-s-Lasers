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


    private bool slidingInput = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {

        if (slidingInput && !pm.sliding && !pm.crouching)
            StartSlide();

        if (!slidingInput && (pm.sliding || pm.crouching) && !pm.somethingAbove)
            StopSlide();
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
        pm.playerHeight /= 2f;
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        if (pm.grounded)
        {
            rb.AddForce(Vector3.down * 50f, ForceMode.Impulse);
        }
        
        
        
    }

    private void StopSlide()
    {
        pm.sliding = false;
        pm.crouching = false;
        pm.currentTimeCrouchBuff = 0;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        pm.playerHeight *=2f;
    }

    private void OnSliding()
    {
        if (!SceneController.isPaused)
            //press and release
            slidingInput = !slidingInput;
    }
}
