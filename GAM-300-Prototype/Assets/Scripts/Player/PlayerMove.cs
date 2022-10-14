using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    IDLE,
    RUNNING,
    DASHING,
    AIR
}

public class PlayerMove : MonoBehaviour
{
    public MovementState state;

    #region Movemment
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float dashSpeed = 10f;
    public float groundDrag = 3f;

    public float jumpForce = 3f;
    public float jumpCoolDown = 0.25f;
    public float airMultiplier = 0.5f;

    bool canJump;
    bool isDashing;
    bool jump;
    bool dash;

    public Transform orientation;
    public Transform pivotPoint;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashUpwardsForce;
    Vector3 delayDashForce;

    public float dashDuration = 0.2f; // how long in dash animation.
    public float dashCoolDownTime = 0.5f;

    float dashCDTimer;

    public KeyCode dashKey = KeyCode.X;


    [Header("Momentum")]
    MovementState lastState;

    float desiredSpeed;
    float lastDesiredSpeed;

    bool toKeepMomentum;

    #endregion

    #region Ground and Slope Checks
    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask groundMask;
    public bool isGrounded;

    [Header("Slope Check")]
    public float maxSlopeAngle = 30f;
    RaycastHit slopeHitInfo;
    bool isExitingSlope;
    #endregion

    [Header("References")]
    PlayerController pc;
    MeshTrailRenderer meshTrailRenderer;

    void Start()
    {
        canJump = true;
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        meshTrailRenderer = GetComponent<MeshTrailRenderer>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        GroundCheck();
        CheckInput();
        StateHandler();

        if (dashCDTimer > 0) dashCDTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Movement();

        if (jump) Jump();
        if (dash) Dash();
    }

    void CheckInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && isGrounded && canJump)
        {
            canJump = false;
            jump = true;
        }

        if (Input.GetKeyDown(dashKey))
        {
            dash = true;
        }
    }

    void StateHandler()
    {
        if (isDashing)
        {
            state = MovementState.DASHING;
            pc.playerAnim.SetTrigger("Dash");
        }

        else if (isGrounded)
        {
            rb.drag = groundDrag;
            ResetJump();

            if (moveDirection.magnitude > 0.1f)
            {
                state = MovementState.RUNNING;
                pc.playerAnim.SetBool("isRunning", true);
            }
            else
            {
                pc.playerAnim.SetBool("isRunning", false);
                state = MovementState.IDLE;
            }

        }
        else
        {
            rb.drag = 0f;
            state = MovementState.AIR;
        }
    }

    void Movement()
    {
        // calculate move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope calculation
        if (OnSlope() && !isExitingSlope)
        {
            rb.AddForce(GetSlopeMovementDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force); // ensures that the player stays on the slope
        }

        if (isGrounded) rb.velocity = moveDirection.normalized * moveSpeed;

        else if (!isGrounded)
        {
            Vector3 normalJumpVector = moveDirection.normalized;
            rb.velocity = new Vector3(normalJumpVector.x * moveSpeed * airMultiplier, rb.velocity.y, normalJumpVector.z * moveSpeed * airMultiplier);
        }

        rb.useGravity = !OnSlope(); // turning gravity on and off depending if player is on slope.
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(pivotPoint.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
    }

    #region Jump Functions
    void Jump()
    {
        jump = false;
        isExitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // reset y velo to 0 to ensure always jump same height.
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        canJump = true;
        isExitingSlope = false;
    }

    #endregion

    #region Dash Functions

    void Dash()
    {
        if (dashCDTimer > 0) return;
        else dashCDTimer = dashCoolDownTime;

        isDashing = true;
        dash = false;

        if (!meshTrailRenderer.isTrailActive)
        {
            meshTrailRenderer.isTrailActive = true;
            StartCoroutine(meshTrailRenderer.RenderMeshTrail(dashDuration));
        }

        Vector3 dashForceToApply = orientation.forward * dashForce + orientation.up * dashUpwardsForce;

        delayDashForce = dashForceToApply;

        Invoke(nameof(DelayDashForce), 0.25f);
        Invoke(nameof(ResetDash), dashDuration);
    }


    void DelayDashForce()
    {
        rb.AddForce(delayDashForce, ForceMode.Impulse);
    }

    void ResetDash()
    {
        isDashing = false;
    }

    #endregion

    #region Slope Functions
    bool OnSlope()
    {
        if (Physics.Raycast(pivotPoint.position, Vector3.down, out slopeHitInfo, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHitInfo.normal); // calculate how steep the slope is.
            return angle > maxSlopeAngle && angle != 0; // if the angle is greatter than the max slope, return true.
        }

        return false; // return false if nothing detected.
    }

    Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHitInfo.normal).normalized;
    }

    #endregion
}
