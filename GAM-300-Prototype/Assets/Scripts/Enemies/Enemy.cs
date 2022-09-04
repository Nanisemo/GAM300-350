using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK
}

public class Enemy : MonoBehaviour, IEnemy, IDamagable
{
    public NavMeshAgent agent;
    public EnemyState state = EnemyState.IDLE;

    bool hasReachedDestination;

    [SerializeField] EnemySetUp enemyConfig;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] Transform[] wayPointArray;
    int index;

    void Start()
    {
        enemyConfig.idleTimer = 0f;
        enemyConfig.patrolTimer = 0f;
    }


    void Update()
    {
        if (GlobalBool.isGameOver || GlobalBool.isPaused) return;

        switch (state)
        {
            case EnemyState.IDLE: IdleBehaviour(); break;
            case EnemyState.PATROL: PatrolBehaviour(); break;
            case EnemyState.CHASE: ChaseBehaviour(); break;
            case EnemyState.ATTACK: AttackBehaviour(); break;
        }

        if (state == EnemyState.IDLE || state == EnemyState.PATROL) IsTargetInRange();
        if (IsTargetInRange()) state = EnemyState.CHASE;
    }

    bool IsTargetInRange() // is target within range of enemy?
    {
        if (Vector3.Distance(enemyConfig.targetTransform.position, transform.position) < enemyConfig.detectionRange)
        {
            ChaseBehaviour();
            return true;
        }
        // need to set player back to a non chasing state.
        return false;
    }

    public void IdleBehaviour()
    {
        enemyConfig.idleTimer += Time.deltaTime;

        if (enemyConfig.idleTimer >= enemyConfig.idleDuration)
        {
            state = EnemyState.PATROL;
            enemyAnimator.SetBool("isPatrol", true);
            hasReachedDestination = false;
            enemyConfig.idleTimer = 0f;
        }
        else enemyAnimator.SetBool("isPatrol", false);

    }

    public void PatrolBehaviour() // patrol around set points. return to this state when target leaves range.
    {
        enemyConfig.patrolTimer += Time.deltaTime;

        if (agent.remainingDistance <= agent.stoppingDistance && !hasReachedDestination)
        {

            agent.SetDestination(wayPointArray[index].position);

            if (Vector3.Distance(wayPointArray[index].position, transform.position) <= 1f)
            {
                index++;
                hasReachedDestination = true;
                print("has reached!");

                if (index > wayPointArray.Length - 1)
                {
                    index = 0;
                }
            }

            print(index);
        }

        if (hasReachedDestination)
        {
            state = EnemyState.IDLE;
            enemyAnimator.SetBool("isPatrol", false);
            print("WEHHHH");
        }
        else enemyAnimator.SetBool("isPatrol", true);



        //if (enemyConfig.patrolTimer >= enemyConfig.patrolDuration)
        //{

        //    enemyAnimator.SetBool("isPatrol", false);
        //    enemyConfig.patrolTimer = 0f;
        //}

    }

    public void ChaseBehaviour()
    {

    }

    public void AttackBehaviour()
    {

    }

    public void TakeDamage(float damageAmount) // when enemy takes damage
    {
        enemyConfig.health -= damageAmount;
        if (enemyConfig.health <= 0) Death();

    }

    public void Death()
    {
        enemyConfig.isKilled = true;

        // add death anim, vfx, sounds here
    }
}
