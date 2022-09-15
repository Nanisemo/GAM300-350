using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    public GameObject timeSlowVolume;
    ComboCheck comboCheck;
    CharacterController charaController;
    TimeSystem timeSystem;
    Camera mainCam;

    #region Movement Variables

    // GENERAL MOVEMENT VARIABLES
    public float moveSpeed;
    public float attackMoveSpeed;
    Vector3 velocity;
    Vector3 direction;
    Vector3 heading;

    // DASH VARIABLES
    public bool dashOnCoolDown;
    public bool isInDash;

    public float dashSpeed;
    public float dashTime = 0.2f; // how long in dash animation.
    public float dashCoolDownTime = 0.1f;

    float gravity = 4f;

    #endregion

    #region Health & Attack

    bool isAttacking;

    public float damage = 1f;
    public float currentHealth;
    float maxHealth = 5;

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

    void Awake()
    {
        timeSystem = GameObject.Find("Game Manager").GetComponent<TimeSystem>();
    }

    void Start()
    {
        timeSlowVolume.SetActive(false);
        charaController = GetComponent<CharacterController>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        meshTrailRenderer = GetComponent<MeshTrailRenderer>();
        comboCheck = GameObject.Find("ComboData").GetComponent<ComboCheck>();

        currentHealth = maxHealth;

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
            Aim(0); // light punch
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Aim(1); // heavy punch
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !dashOnCoolDown)
        {
            StartCoroutine(Dash());
        }
        // TODO: make player face the mouse cursor when clicked. [DONE, left bugfix. angle offset is not in sets of 90 degs]
        // TODO: make a basic combat system that uses LMB & RMB [ANIMS DONE]
        // TODO: player anim to also have the damage hitbox enabled. >> need to duplicate the animation clip and reassign onto the animator.
        // TODO: import terrence's combat/combo sys if needed.

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
                charaController.Move(direction.normalized * Time.unscaledDeltaTime * moveSpeed); // player cannot move when attacking.
        }
        else playerAnim.SetBool("isRunning", false);

        velocity.y -= gravity * Time.unscaledDeltaTime; // ensure that the player is grounded at all times.
        charaController.Move(velocity * Time.unscaledDeltaTime);
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
                isInDash = true;
                charaController.Move(transform.forward * Time.deltaTime * dashSpeed); // dash in the direction that the player is facing.

                if (!meshTrailRenderer.isTrailActive)
                {
                    meshTrailRenderer.isTrailActive = true;
                    StartCoroutine(meshTrailRenderer.RenderMeshTrail(dashTime));
                }

                yield return null;
            }
        }

        if (dashOnCoolDown) // Dash CD
        {
            isInDash = false;
            yield return new WaitForSeconds(dashCoolDownTime);
            dashOnCoolDown = false;
            meshTrailRenderer.isTrailActive = false;
            print("dash reset");
        }
    }

    #endregion

    #region Health, Damage Taken & Death Functions
    void CheckHealth()
    {
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (currentHealth <= 0) PlayerDeath();

        // to remove  for build
        if (Input.GetKeyDown(KeyCode.F) && currentHealth != 0) currentHealth--;
        if (Input.GetKeyDown(KeyCode.R) && currentHealth != 5) currentHealth++;

    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
    }

    void PlayerDeath()
    {
        playerAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerAnim.Play("Player Death");
        currentHealth = 0;
        GlobalBool.isGameOver = true;
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

    void Aim(int x) // x determines type of punch
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

            charaController.Move(direction.normalized * Time.unscaledDeltaTime * attackMoveSpeed);

            if (x == 0)
            {
                playerAnim.SetTrigger("lightPunch");
                comboCheck.AddToQueue("light_punch");
            }
            else
            {
                playerAnim.SetTrigger("heavyPunch");
                comboCheck.AddToQueue("heavy_punch");
            }

            StartCoroutine(CanAttackAgain());
        }
    }

    IEnumerator CanAttackAgain()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider hitInfo)
    {


        if (hitInfo.gameObject.CompareTag("Enemy Hurtbox"))
        {
            Enemy thisEnemy = hitInfo.gameObject.transform.parent.gameObject.GetComponent<Enemy>(); // getting script from parent obj. hurtbox is a child.

            if (isInDash)
            {
                timeSystem.TimeFracture();
            }
            else
            {
                TakeDamage(thisEnemy.enemyConfig.damage);
                print("ouchie ouch");
                playerAnim.Play("Hit");
            }

        }

    }

}
