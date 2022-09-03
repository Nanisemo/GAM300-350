using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController charaController;
    Camera mainCam;

    #region Movement Variables

    // GENERAL MOVEMENT VARIABLES
    public float moveSpeed;
    public float attackMoveSpeed;
    Vector3 velocity;
    Vector3 direction;
    Vector3 heading;

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

    #region Animation

    Animator playerAnim;

    #endregion

    #region MISC

    MeshTrailRenderer meshTrailRenderer;

    #endregion

    [SerializeField] LayerMask groundMask;
    bool isAttacking;

    public int currentHealth;
    int maxHealth = 5;

    void Start()
    {
        charaController = GetComponent<CharacterController>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerAnim = GameObject.Find("Player").GetComponent<Animator>();
        meshTrailRenderer = GetComponent<MeshTrailRenderer>();

        currentHealth = maxHealth;

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

        CheckHealth();

        if (GlobalBool.isGameOver || GlobalBool.isPaused) return; // player unable to move if either bool is true.

        PlayerMove();

        if (Input.GetMouseButtonDown(0)) // rotates the player when clicked.
        {
            Aim();
            print("LMB Clicked");
        }
        else isAttacking = false;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            StartCoroutine(Dash());
        }

        // TODO: make player face the mouse cursor when clicked. [DONE, left bugfix. angle offset is not in sets of 90 degs]
        // TODO: make a basic combat system that uses LMB.
    }

    #region Movement Functions

    void PlayerMove()
    {

        if (direction.magnitude >= 0.1f)
        {
            // rotate logic
            heading = direction.normalized;
            transform.forward = heading;
            playerAnim.SetBool("isRunning", true);

            if (!isAttacking)
                charaController.Move(direction.normalized * Time.deltaTime * moveSpeed); // player cannot move when attacking.
        }
        else playerAnim.SetBool("isRunning", false);

        velocity.y -= gravity * Time.deltaTime; // ensure that the player is grounded at all times.
        charaController.Move(velocity * Time.deltaTime);
    }

    IEnumerator Dash() // activated when Space is pressed.
    {
        float startTime = Time.time;

        if (!dashOnCoolDown)
        {
            while (Time.time < startTime + dashTime)
            {
                dashOnCoolDown = true;

                playerAnim.SetTrigger("Dash");
                charaController.Move(transform.forward * Time.deltaTime * dashSpeed); // dash in the direction that the player is facing.
                if (!meshTrailRenderer.isTrailActive) StartCoroutine(meshTrailRenderer.RenderMeshTrail(dashTime));
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

    #region Health Functions
    void CheckHealth()
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (currentHealth <= 0 && !GlobalBool.isGameOver)
        {
            currentHealth = 0;
            GlobalBool.isGameOver = true;
            playerAnim.SetTrigger("isDead");

        }

        if (Input.GetKeyDown(KeyCode.F) && currentHealth != 0) // to remove  for build
        {
            currentHealth--;

        }
        if (Input.GetKeyDown(KeyCode.R) && currentHealth != 5) currentHealth++;

        print(currentHealth);
    }
    #endregion

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }

    void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            isAttacking = true;

            // Calculate the direction
            var direction = position - transform.position;

            // Ignore the height difference.
            direction.y = 0;

            // Make the transform look in the direction.
            var padding = direction + heading;

            transform.forward = padding;

            charaController.Move(direction.normalized * Time.deltaTime * attackMoveSpeed);
            StartCoroutine(CanAttackAgain());
        }
    }

    IEnumerator CanAttackAgain()
    {
        yield return new WaitForSecondsRealtime(1f);
        isAttacking = false;
    }
}
