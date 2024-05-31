using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    //private float sprintSpeed;
    public float crouchSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchYScale;
    public float durationCrouchBuff = 3f;
    public float buffCrouchAmount = 3f;
    private float crouchBuff = 5f;
    private float startYScale;
    private float currentTimeCrouchBuff = 0;
    private bool somethingAbove = false;
    private bool buff;
    [Tooltip("Define a variable to control the exponential growth rate")]
    public float exponentialGrowthRateBuff = 0.1f;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;


    [HideInInspector] public Vector2 inputMove;
    bool jumpingInput = false, crouchingInput = false;
    
    Vector3 moveDirection;

    Rigidbody rb;
    private MovementState state;

    private float timer;

    [Header("DEBUG")]
    [SerializeField] private bool DEBUG = false;
    

    public enum MovementState
    {
        idle,
        walking,
        sprinting,
        air,
        crouching
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        //if (DEBUG) RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(transform.position, 0.5f, Vector3.down, playerHeight * 0.4f, preview: RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor);

        grounded = Physics.SphereCast(transform.position, 0.5f,Vector3.down, out RaycastHit hit, playerHeight * 0.4f, whatIsGround);
        
        MyInputs();
        SpeedControl();
        StateHandler();
        // handle drag
        if (grounded) rb.drag = groundDrag; //(remove this for on ice movement!!)
        else rb.drag = 0f;

        EventManager.Player.OnSpeedChanged.Invoke(this, new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude);
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MyInputs()
    {

        //when to jump
        
        if (jumpingInput && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        //start crouch
        if (crouchingInput && state != MovementState.crouching)
        {
            if (state == MovementState.idle && moveSpeed == 0)
            {
                buff = false;
            }
            else
            {
                buff = true;
            }
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        //stop crouch
        if (!crouchingInput && state == MovementState.crouching)
        {
            if (DEBUG) RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(transform.position, 0.5f, Vector3.up, playerHeight * 0.5f + 0.2f, preview: RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor) ;
            somethingAbove = Physics.SphereCast(transform.position, 0.5f, Vector3.up, out RaycastHit hit, playerHeight * 0.5f + 0.2f);
            
            if (!somethingAbove)
            {
                currentTimeCrouchBuff = 0;
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }           
        }
    }

    private void StateHandler()
    {
        //Mode - crouching
        if (crouchingInput || somethingAbove)
        {
            state = MovementState.crouching;

            if (buff)
            {
                currentTimeCrouchBuff += Time.deltaTime;

                float interpolation = currentTimeCrouchBuff / durationCrouchBuff;
                crouchBuff = Mathf.Lerp(buffCrouchAmount, 0, interpolation);

                // Calculate the exponential change in speed of movement
                float exponentialChange = Mathf.Pow(2, exponentialGrowthRateBuff * currentTimeCrouchBuff);

                // Apply exponential change to motion
                moveSpeed = (crouchSpeed* inputMove.magnitude) + crouchBuff* exponentialChange;
}
            else
            {
                moveSpeed = (crouchSpeed * inputMove.magnitude);
            }
            
        }
        /**
        //Mode - sprinting
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        */
        //Mode - walking
        else if (grounded)
        {
            if (rb.velocity.magnitude > 0.4f) state = MovementState.walking;
            else state = MovementState.idle;
            moveSpeed = walkSpeed * inputMove.magnitude;
        }

        //Mode - Air
        else
        {
            moveSpeed = walkSpeed;
            state = MovementState.air;
        }
        //if (!SceneController.isPaused) 
            //EventManager.Player.OnMovementStateChanged.Invoke(this, state);


    }


    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * inputMove.y + orientation.right * inputMove.x;

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0 || rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force); // keep the player constantly on the slope without bouncing
            }
        }

        //on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //turn gravity off while on slope
        rb.useGravity = !OnSlope();

        
    }

    private void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope() && !exitingSlope && !crouchingInput)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        //limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        
        

    }

    private void Jump()
    {
        exitingSlope = true;

        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
        jumpingInput = false;
    }

    private bool OnSlope()
    {
        //if (DEBUG) RotaryHeart.Lib.PhysicsExtension.Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor);
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //if (DEBUG) Debug.Log("slope: " + (angle < maxSlopeAngle && angle != 0).ToString());
            return angle < maxSlopeAngle && angle != 0;
            
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void OnJump()
    {
        if (!SceneController.isPaused)
            //press and release
            jumpingInput = !jumpingInput;
            
    }

    private void OnMove(InputValue inputValue)
    {
        inputMove = inputValue.Get<Vector2>();
    }

    private void OnCrouch()
    {
        if (!SceneController.isPaused)
            //press and release
            crouchingInput = !crouchingInput;
    }
}
