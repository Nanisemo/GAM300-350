using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Movement Variables
    public float moveSpeed;
    public float dashSpeed;
    float gravity = 48f;
    float dashCoolDown = 0.5f;
    bool isDashing;


    //ROTATION VECTORS
    Vector3 forwardVector;
    Vector3 rightVector;
    #endregion

    #region Ground Check
    public bool isGrounded;
    Transform groundCheck;
    float checkRadius = 0.4f;
    public LayerMask groundMask;
    #endregion


    void Start()
    {

        groundCheck = GameObject.Find("Ground Check").GetComponent<Transform>();

        #region Initializating Iso Rotation
        forwardVector = Camera.main.transform.forward; // setting the player's forward to be the same as camera.
        forwardVector.y = 0f;
        forwardVector = Vector3.Normalize(forwardVector);
        rightVector = Quaternion.Euler(new Vector3(0, 90, 0)) * forwardVector;
        #endregion
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundMask);

        // FUTURE: FIX THE RIGID ROTATION. ROTATION IS NOW IN STEPS OF 90 DEGS.
        PlayerMove(); // if can use rigidbody to move, even better.

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    void PlayerMove() // somehow the player is moving by itself without input lmao - val.
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (!isGrounded)
        {
            Vector3 currentHeight = new Vector3(0, transform.position.y, 0);
            currentHeight.y -= gravity * Time.deltaTime;
            print("going down!");

        }

        if (direction.magnitude >= 0.1f)
        {
            Vector3 heading = direction.normalized;

            transform.forward = heading;
            transform.position += heading * moveSpeed * Time.deltaTime;
        }

    }

    #region Dash Functions

    void Dash() // activated when Space is pressed.
    {
        //TODO: MAKE PLAYER DASH IN THE DIR ITS FACING
        isDashing = true;

        print("Dashed!");
        StartCoroutine(ResetDash());
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(dashCoolDown);
        isDashing = false;
        print("Dash resetted!");
    }

    #endregion
}
