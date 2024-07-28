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
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    [SerializeField] private float slopeAngleChangeSpeed = 50f;

    public Transform orientation;

    [HideInInspector] public Vector2 inputMove;
    private bool jumpingInput = false;


    Vector3 moveDirection;

    Rigidbody rb;

    [SerializeField] private bool DEBUG = false;

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
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hit, playerHeight * 0.4f, whatIsGround);

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
        if (!crouching && state != MovementState.crouching)
        {
            if (DEBUG) RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(transform.position, 0.5f, Vector3.up, playerHeight * 0.5f + 0.2f, preview: RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor);
            somethingAbove = Physics.SphereCast(transform.position, 0.5f, Vector3.up, out RaycastHit hit, playerHeight * 0.5f + 0.2f);

            if (!somethingAbove)
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }
        }
    }

    private void StateHandler()
    {
        // Mode - Sliding
        if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = maxSlopeSpeed;

            else
            {
                currentTimeCrouchBuff += Time.deltaTime;

                float interpolation = currentTimeCrouchBuff / durationCrouchBuff;
                crouchBuff = Mathf.Lerp(buffCrouchAmount, 0, interpolation);

                // Calculate the exponential change in speed of movement
                float exponentialChange = Mathf.Pow(2, exponentialGrowthRateBuff * currentTimeCrouchBuff);

                // Apply exponential change to motion
                desiredMoveSpeed = crouchSpeed + crouchBuff * exponentialChange;
                //desiredMoveSpeed = sprintSpeed;
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

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * inputMove.y + orientation.right * inputMove.x;

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
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
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