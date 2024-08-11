using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector] public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float maxSlopeSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    [HideInInspector] public bool somethingAbove = false;
    [HideInInspector] public bool crouching = false;

    
    

    [Header("Sliding")]
    [Tooltip("Define a variable to control the exponential growth rate")]
    public float exponentialGrowthRateBuff = 0.1f;
    [HideInInspector] public float currentTimeCrouchBuff = 0;
    public float durationCrouchBuff = 3f;
    public float buffCrouchAmount = 3f;
    private float crouchBuff = 5f;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [HideInInspector] public bool grounded;
    private bool groundedCheck;
    [Range(0.0f, 0.25f)]
    [SerializeField] private float coyoteTime = 0.1f;
    private float _coyoteTime;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    [SerializeField] private float slopeAngleChangeSpeed = 50f;

    public Transform orientation;

    [HideInInspector] public Vector2 inputMove;
    private bool jumpingInput = false;

    [Tooltip("How fast speed decay when you stand up from a slide")]
    [SerializeField] private float WalkDecayMultiply = 5;

    Vector3 moveDirection;

    Rigidbody rb;
    private LayerMask ignorePlayerBodyLayer;

    [SerializeField] private bool DEBUG = false;
    private Sliding slidingScript;

    [HideInInspector] public MovementState state;
    public enum MovementState
    {
        idle,
        walking,
        sprinting,
        crouching,
        sliding,
        air
    }

    public bool sliding;



    private void Start()
    {
        slidingScript = GetComponent<Sliding>();
        _coyoteTime = coyoteTime;
        ignorePlayerBodyLayer = LayerMask.GetMask("IgnorePickUpRay");
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        if (DEBUG) RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(transform.position,radius: 0.5f,direction: Vector3.down, maxDistance: playerHeight * 0.4f, preview: RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor, drawDuration: 0.1f) ;
        groundedCheck = Physics.SphereCast(transform.position, 0.45f, Vector3.down, out RaycastHit hit, playerHeight * 0.4f, whatIsGround);

        //COYOTE TIME
        if (!groundedCheck)
        {
            if (readyToJump)
            {
                _coyoteTime -= Time.deltaTime;
                if (_coyoteTime <= 0)
                {
                    grounded = false;
                }
            }
            else
            {
                grounded = false;
            }
            
        }
        else
        {
            _coyoteTime = coyoteTime;
            grounded = true;
        }

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag; //(remove this for on ice movement!!)
        else
            rb.drag = 0;

        EventManager.Player.OnSpeedChanged.Invoke(this, new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {


        // when to jump
        if(jumpingInput && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }


        // stop crouch
        if ((crouching || sliding) && (state != MovementState.crouching || state != MovementState.sliding))
        {
            if (DEBUG) RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(transform.position, 0.5f, Vector3.up, playerHeight * 0.5f + 0.2f, preview: RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor);
            somethingAbove = Physics.SphereCast(transform.position, 0.5f, Vector3.up, out RaycastHit hit, playerHeight * 0.5f + 0.2f);
        }
    }

    private void StateHandler()
    {
        // Mode - Sliding
        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = maxSlopeSpeed;
            }
            else
            {
                //slide on ground

                currentTimeCrouchBuff += Time.deltaTime;

                float interpolation = currentTimeCrouchBuff / durationCrouchBuff;
                crouchBuff = Mathf.Lerp(buffCrouchAmount, 0, interpolation);

                // Calculate the exponential change in speed of movement
                float exponentialChange = Mathf.Pow(2, exponentialGrowthRateBuff * currentTimeCrouchBuff);

                // Apply exponential change to motion
                desiredMoveSpeed = crouchSpeed + crouchBuff * exponentialChange;
                //desiredMoveSpeed = sprintSpeed;
            }

            if (rb.velocity.magnitude <= crouchSpeed && !slidingScript.forceSlide)
            {
                state = MovementState.crouching;
                sliding = false;
                crouching = true;
                StopAllCoroutines();
                moveSpeed = 0;
                desiredMoveSpeed = crouchSpeed;
            }

            if (slidingScript.forceSlide && rb.velocity.magnitude <= crouchSpeed)
            {
                moveSpeed = 0;
            }
        }

        // Mode - Crouching
        else if (crouching)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            if (rb.velocity.magnitude > 0.4f) state = MovementState.walking;
            else
            {
                state = MovementState.idle;
                desiredMoveSpeed = 0;
                StopAllCoroutines();
            }
                
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desiredMoveSpeed has changed drastically
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        if (!SceneController.isPaused)
            EventManager.Player.OnMovementStateChanged.Invoke(this, state);
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / (slopeAngleChangeSpeed - slopeAngle));
                
                if (rb.velocity.y <= 0)
                {
                    if (state == MovementState.walking)
                        time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease * WalkDecayMultiply;
                    else
                        time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
                }
                else
                {
                    if (state == MovementState.walking)
                        time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease * WalkDecayMultiply * 2f;
                    else
                        time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease * 2f;
                }
                

                

                
            }
            else
            {
                if (state == MovementState.walking)
                    time += Time.deltaTime * speedIncreaseMultiplier * WalkDecayMultiply;
                else
                    time += Time.deltaTime * speedIncreaseMultiplier;
                
            }

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        
        if (sliding)
        {
            float horizontalMultiplier = Mathf.Lerp(1.1f, 0.05f, moveSpeed / (maxSlopeSpeed - 5f));
            if(DEBUG)Debug.Log(horizontalMultiplier);
            moveDirection = orientation.forward * inputMove.y + orientation.right * inputMove.x * horizontalMultiplier;
        }
        else
        {
            moveDirection = orientation.forward * inputMove.y + orientation.right * inputMove.x;
        }


        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);


            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
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

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
        jumpingInput = false;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, ~ignorePlayerBodyLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }


    private void OnJump()
    {
        if (!SceneController.isPaused && grounded)
            //press and release
            jumpingInput = !jumpingInput;

    }

    private void OnMove(InputValue inputValue)
    {
        inputMove = inputValue.Get<Vector2>();
    }

}