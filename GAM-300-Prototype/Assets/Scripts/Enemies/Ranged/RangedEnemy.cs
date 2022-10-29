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
    // LONGER IDLE AND SHORTER PATROL ROUTES.

    public NavMeshAgent agent;
    public EnemySetUp enemyConfig;
    public RangeEnemyState state = RangeEnemyState.IDLE;

    [SerializeField] float moveSpeed;
    [SerializeField] Transform[] wayPointArray;

    float health;

    bool hasReachedDestination;

    public bool isKilled,
            isStunned;

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

    public Transform firePoint;
    public GameObject bulletPrefab;

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
        enemyConfig.patrolTimer = 0f;
        bulletLeft = maxBullet;

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
        }

    }

    #region Bool Functions
    bool IsInDetectionRange()
    {
        if (!enemyConfig.targetTransform) return false;
        if (Vector3.Distance(enemyConfig.targetTransform.position, transform.position) <= enemyConfig.detectionRange)
        {
            print("in detection range");
            return true;
        }
        else
        {
            //enemyAnimator.SetBool("isAttacking", false);
            state = RangeEnemyState.PATROL;
            return false;
        }

    }

    #endregion

    #region Behaviour Logic
    public void IdleBehaviour()
    {
        SetAggro(false);
    }

    public void PatrolBehaviour()
    {
        SetAggro(false);
    }

    public void ReloadBehaviour()
    {

    }

    public void ChaseBehaviour()
    {

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
    }

    public void Death()
    {
        SetAggro(false);

        state = RangeEnemyState.DEAD;
        isKilled = true;
        FreezePosition();
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
        UnFreezePosition();

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
        FreezePosition();
        isReloading = true;

        bulletLeft = maxBullet;
        bulletShot = 0;

        print("reloading");

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
        print("dont reloading");
    }

}
