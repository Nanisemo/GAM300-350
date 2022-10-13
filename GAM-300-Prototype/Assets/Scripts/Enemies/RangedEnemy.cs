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
    // NO CHASING AFTER PLAYER.
    // SHOOTS PROJECTILES AT PLAYER WHEN WITHIN DETECTION RANGE.
    // STOPS MOVING AFTER EVERY 5 PROJECTILES SHOT TO RELOAD.
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
    int bulletCounter;
    public Transform firePoint;

    void Awake()
    {
        isKilled = false;
    }

    void Start()
    {
        health = enemyConfig.health;
        moveSpeed = agent.speed;
        enemyConfig.idleTimer = 0f;
        enemyConfig.patrolTimer = 0f;
        enemyConfig.targetTransform = GameObject.FindGameObjectWithTag(enemyConfig.targetTag).GetComponent<Transform>();
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
        }

    }

    bool IsInAtkRange()
    {
        if (Vector3.Distance(enemyConfig.targetTransform.position, transform.position) <= enemyConfig.attackRange)
        {
            return true;
        }
        else
        {
            //enemyAnimator.SetBool("isAttacking", false);
            return false;
        }

    }

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
    }

    public void StunnedBehaviour()
    {
        SetAggro(true);
    }

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

    public void FreezePosition()
    {
        agent.isStopped = true;
    }

    public void UnFreezePosition()
    {
        agent.isStopped = false;
    }


    public void SetAggro(bool aggroed)
    {
        if (aggroed) GlobalBool.SetInRangeCombat(this, true);
        else GlobalBool.SetInRangeCombat(this, false);
    }

}
