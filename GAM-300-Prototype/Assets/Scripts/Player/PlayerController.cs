using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    Vector3 externalMovement;
    Rigidbody myCartRB;

    #region Health & Attack

    public bool isAttacking;

    public float damage = 1f;
    public float currentHealth;
    public float maxHealth = 5;

    #endregion


    #region Animation

    public Animator playerAnim;
    #endregion

    public GameObject buffVFXTest;
    public Transform VFXPoint;

    #region MISC
    TimeSystem timeSystem;
    #endregion


    void Awake()
    {
        timeSystem = GameObject.Find("Game Manager").GetComponent<TimeSystem>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {

        CheckHealth();

        if (GlobalBool.isGameOver || GlobalBool.isPaused || GlobalBool.isLoading) return; // player unable to move if either bool is true.

        if (Input.GetKeyDown(KeyCode.V)) timeSystem.TimeFracture();

        if (Input.GetKeyDown(KeyCode.LeftShift)) ActivateBuff();
    }

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
        playerAnim.Play("Hit");
    }

    void PlayerDeath()
    {
        playerAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerAnim.Play("Player Death");
        currentHealth = 0;
        GlobalBool.isGameOver = true;
        GlobalBool.enemiesInCombat.Clear();
    }

    #endregion

    #region Trigger Collision Stuff

    private void OnTriggerEnter(Collider hitInfo)
    {

        //if (hitInfo.CompareTag("Dash Window"))
        //{
        //    if (isInDash)
        //    {
        //        timeSystem.TimeFracture();
        //    }
        //}

        if (hitInfo.gameObject.CompareTag("Enemy Hurtbox") /*&& !isInDash*/)
        {
            Enemy thisEnemy = hitInfo.gameObject.transform.parent.gameObject.GetComponent<Enemy>(); // getting script from parent obj. hurtbox is a child.

            TakeDamage(thisEnemy.enemyConfig.damage);
            print("ouchie ouch");

        }

        if (hitInfo.CompareTag("Cart Handle"))
        {
            CartMove moveCart = hitInfo.GetComponent<CartMove>();
            transform.position = GetComponentInParent<Transform>().position; //snap to cart middle

            StartCoroutine(moveCart.Interact(gameObject.transform));
        }

        if (hitInfo.CompareTag("Cart"))
        {
            myCartRB = hitInfo.gameObject.GetComponent<Rigidbody>();
            // transform.SetParent(hitInfo.transform);
            Debug.Log("attach");

        }

        if (hitInfo.CompareTag("Death Zone"))
        {
            PlayerDeath();
        }

    }

    private void OnTriggerStay(Collider hitInfo)
    {
        //if (hitInfo.CompareTag("Dash Window"))
        //{
        //    if (isInDash)
        //    {
        //        timeSystem.TimeFracture();
        //    }
        //}
    }

    private void OnTriggerExit(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Cart"))
        {
            // transform.SetParent(null);

            myCartRB = null;
            Debug.Log("remove");
        }
    }

    #endregion

    #region TEMPO FUNCTIONS

    void ActivateBuff()
    {
        playerAnim.Play("Buff");
        Instantiate(buffVFXTest, VFXPoint.position, Quaternion.identity);
    }

    #endregion

}
