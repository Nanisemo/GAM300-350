using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RangeEnemyState
{
    IDLE,
    PATROL,
    RELOAD,
    ATTACK,
    STUNNED,
    DEAD
}
public class RangedEnemy : MonoBehaviour, IEnemy, IDamagable
{
    //ENEMY BEHAVIOUR
    // NO CHASING AFTER PLAYER.- done
    // SHOOTS PROJECTILES AT PLAYER WHEN WITHIN DETECTION RANGE. - done
    // STOPS MOVING AFTER EVERY 5 PROJECTILES SHOT TO RELOAD. - done
    // MOVES AWAY FROM PLAYER WHEN PLAYER IS TOO CLOSE.
    // LONGER IDLE AND SHORTER PATROL ROUTES. - done

    [Header("AI Configs")]
    public NavMeshAgent agent;
    public EnemySetUp enemyConfig;
    public RangeEnemyState state = RangeEnemyState.IDLE;

    public Collider col;

    Enemies.EnemyYeet yeet;
    public Vector3 yeetForce = new Vector3(.5f, .5f, .5f);

    [SerializeField] float moveSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float damageTimeOut = 1f;
    [SerializeField] Transform[] wayPointArray;
    public int index;

    float health;

    bool hasReachedDestination;


    Animator anim;

    public bool isKilled,
            isStunned, damageTaken;

    #region Shooting
    [Header("Shooting")]
    // public GameObject bulletPrefab;
    public float reloadTime = 3f;
    public float fireRate = 1f; // 1 bullet per second. higher = slower firing rate.
    public float bulletForce = 32f;
    public float upwardsForce = 8f;

    float shootTimer;

    public int maxBullet = 5;

    int bulletShot;
    int bulletLeft;

    bool canShoot;
    bool isReloading;

    #endregion

    [Header("Others")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    PlayerController pc;

    [Header("Effects")]
    public GameObject hitImpactPrefab;

    [SerializeField] Renderer mr;

    public float flashIntensity = 10f;
    public float flashDuration = 0.2f;
    float flashTimer;

    void Awake()
    {
        isKilled = false;
    }

    void Start()
    {
        canShoot = true;
        health = enemyConfig.health;
        moveSpeed = agent.speed;

        enemyConfig.idleTimer = 0f;


        bulletLeft = maxBullet;
        anim = GetComponent<Animator>();
        yeet = GetComponent<Enemies.EnemyYeet>();
        mr = GetComponentInChildren<Renderer>();
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        var target = GameObject.FindGameObjectWithTag(enemyConfig.targetTag);
        if (target)
        {
            enemyConfig.targetTransform = target.transform;
        }
    }

    void Update()
    {
        if (GlobalBool.isGameOver || GlobalBool.isPaused) return;


        if (!isKilled)
        {
            switch (state)
            {
                case RangeEnemyState.IDLE: IdleBehaviour(); break;
                case RangeEnemyState.PATROL: PatrolBehaviour(); break;
                case RangeEnemyState.ATTACK: AttackBehaviour(); break;
                case RangeEnemyState.RELOAD: ReloadBehaviour(); break;
            }

            if (IsInDetectionRange()) state = RangeEnemyState.ATTACK;

            if (bulletShot <= maxBullet && bulletLeft > 0)
            {
                canShoot = true;
            }
            else canShoot = false;

            FlashEffect();
        }
        else
        {
            col.enabled = false;
        }

    }

    #region Bool Functions
    bool IsInDetectionRange()
    {
        if (!enemyConfig.targetTransform) return false;
        if (Vector3.Distance(enemyConfig.targetTransform.position, transform.position) <= enemyConfig.detectionRange)
        {

            return true;
        }
        else if (state == RangeEnemyState.ATTACK) // return AI back to patrol state if out of detection range
        {
            state = RangeEnemyState.PATROL;
            UnFreezePosition();

            return false;
        }

        return false;

    }

    #endregion

    #region Behaviour Logic
    public void IdleBehaviour()
    {
        enemyConfig.idleTimer += Time.deltaTime;
        agent.speed = moveSpeed;
        SetAggro(false);
        UnFreezePosition();

        if (enemyConfig.idleTimer >= enemyConfig.idleDuration) // after idle timer has elapsed, set state to patrol.
        {
            state = RangeEnemyState.PATROL;
            //  enemyAnimator.SetBool("isPatrol", true);
            hasReachedDestination = false;
            enemyConfig.idleTimer = 0f;
        }

    }

    public void PatrolBehaviour()
    {
        SetAggro(false);

        if (Vector3.Distance(wayPointArray[index].position, transform.position) > enemyConfig.maxChaseDistance) agent.speed = chaseSpeed; else agent.speed = moveSpeed;

        if (agent.remainingDistance <= agent.stoppingDistance && !hasReachedDestination)
        {

            agent.SetDestination(wayPointArray[index].position);

            if (Vector3.Distance(wayPointArray[index].position, transform.position) <= 1f)
            {
                index++;
                hasReachedDestination = true;

                if (index > wayPointArray.Length - 1)
                {
                    index = 0;
                }
            }
        }

        if (hasReachedDestination)
        {
            state = RangeEnemyState.IDLE;
        }
    }

    public void ReloadBehaviour()
    {
        // for any VFX or Anim
    }

    public void ChaseBehaviour()
    {
        SetAggro(true);
    }

    public void AttackBehaviour()
    {
        SetAggro(true);
        StartCoroutine(Aim());
        Shoot();
    }

    public void StunnedBehaviour()
    {
        SetAggro(true);
    }

    #endregion

    #region Damage & Death Functions
    public void TakeDamage(float damageAmount)
    {
        SetAggro(true);

        if (health - damageAmount > 0)
        {

            health -= damageAmount;
            print(health);
        }
        else
        {
            health = 0f;
            Death();
        }

        flashTimer = flashDuration;
    }

    public void Death()
    {
        SetAggro(false);

        state = RangeEnemyState.DEAD;
        isKilled = true;
        anim.Play("Death");
        FreezePosition();
    }

    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player Hitbox") && !isKilled)
        {
            Vector3 relativeForce = transform.position - enemyConfig.targetTransform.position;
            print(relativeForce);
            yeet.Push(relativeForce);
            RangedEnemyTakeDamage();
        }
    }

    IEnumerator DamageFrameDelay()
    {
        yield return new WaitForSecondsRealtime(damageTimeOut);
        damageTaken = false;
    }

    #endregion

    #region Agent Position Function
    public void FreezePosition()
    {
        agent.isStopped = true;
    }

    public void UnFreezePosition()
    {
        agent.isStopped = false;
    }

    #endregion

    #region Misc
    public void SetAggro(bool aggroed)
    {
        if (aggroed) GlobalBool.SetInRangeCombat(this, true);
        else GlobalBool.SetInRangeCombat(this, false);

        //if (aggroed) GlobalBool.isInCombat = true;
        //else GlobalBool.isInCombat = false; ;
    }

    IEnumerator Aim()
    {
        Vector3 relativePos = enemyConfig.targetTransform.position - transform.position;
        relativePos.y = 0f;

        Quaternion lookRotation = Quaternion.LookRotation(relativePos);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, time);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    #endregion

    void Shoot()
    {
        FreezePosition();

        if (canShoot && !isReloading)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer > fireRate)
            {
                Vector3 relativeTargetPos = enemyConfig.targetTransform.position; // getting player last position when this condition is called.

                //instatiate bullet here.
                firePoint.LookAt(relativeTargetPos);

                Rigidbody bulletRb = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
                bulletRb.AddForce(bulletRb.transform.forward * bulletForce, ForceMode.Impulse);
                bulletRb.AddForce(bulletRb.transform.up * upwardsForce, ForceMode.Impulse);

                shootTimer = 0;
                bulletLeft--;
                bulletShot++;
                print("shooting rn");
            }
        }
        else if (!canShoot)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        //FreezePosition();
        isReloading = true;

        bulletLeft = maxBullet;
        bulletShot = 0;

        print("reloading");

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
        print("dont reloading");
    }

    void FlashEffect()
    {
        flashTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(flashTimer / flashDuration);
        float intensity = (lerp * flashIntensity) + 1f;
        mr.material.SetColor("_EmissionColor", Color.white * intensity);

    }

    public void RangedEnemyTakeDamage()
    {
        if (!damageTaken && !isKilled)
        {
            damageTaken = true;
            StartCoroutine(DamageFrameDelay());

            Vector3 hitPointPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            FindObjectOfType<CameraShake>().ShakeCamera();
            FindObjectOfType<HitStop>().StartHitStop();
            Instantiate(hitImpactPrefab, hitPointPos, transform.rotation);
            TakeDamage(pc.damage);

            print("ranged enemy ouchie ouch");
        }

    }

    public void ExecuteAOE()
    {
        StartCoroutine(DelayThisAOE());
    }

    IEnumerator DelayThisAOE()
    {
        yield return new WaitForSecondsRealtime(1f);
        yeet.Push(yeetForce);
        RangedEnemyTakeDamage();
    }

}
