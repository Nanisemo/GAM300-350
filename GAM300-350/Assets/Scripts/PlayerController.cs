using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController charaController;

    #region Movement Variables

    // GENERAL MOVEMENT VARIABLES
    public float moveSpeed;
    Vector3 velocity;
    Vector3 direction;

    // DASH VARIABLES
    bool dashOnCoolDown;
    public float dashSpeed;
    public float dashTime = 0.2f; // how long in dash animation.
    public float dashCoolDownTime = 0.1f;

    float gravity = 4f;

    //ROTATION VECTORS
    Vector3 forwardVector;
    Vector3 rightVector;
    #endregion

    #region Ground Check
    bool cannotMove;
    public bool isGrounded;
    #endregion


    void Start()
    {
        charaController = GetComponent<CharacterController>();

        #region Initializating Iso Rotation
        forwardVector = Camera.main.transform.forward; // setting the player's forward to be the same as camera.
        forwardVector.y = 0f;
        forwardVector = Vector3.Normalize(forwardVector);
        rightVector = Quaternion.Euler(new Vector3(0, 90, 0)) * forwardVector;
        #endregion
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        direction.Normalize();

        isGrounded = charaController.isGrounded;

        PlayerMove();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
    }

    void PlayerMove()
    {

        if (direction.magnitude >= 0.1f)
        {
            // rotate logic
            Vector3 heading = direction.normalized;
            transform.forward = heading;

            charaController.Move(direction.normalized * Time.deltaTime * moveSpeed);
        }

        velocity.y -= gravity * Time.deltaTime; // ensure that the player is grounded at all times.
        charaController.Move(velocity * Time.deltaTime);
    }

    #region Dash Functions

    IEnumerator Dash() // activated when Space is pressed.
    {
        float startTime = Time.time;

        if (!dashOnCoolDown)
        {
            while (Time.time < startTime + dashTime)
            {
                dashOnCoolDown = true;
                charaController.Move(transform.forward * Time.deltaTime * dashSpeed); // dash in the direction that the player is facing.
                yield return null;
            }
        }
        else // Dash CD
        {
            yield return new WaitForSeconds(dashCoolDownTime);
            dashOnCoolDown = false;
            print("dash reset");
        }
    }
    #endregion
}
