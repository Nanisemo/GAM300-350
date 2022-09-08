using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
    STUNNED,
    DEAD
}

public class Enemy : MonoBehaviour, IEnemy, IDamagable
{
    public NavMeshAgent agent;
    public EnemyState state = EnemyState.IDLE;

    bool hasReachedDestination;

    [SerializeField] float moveSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] EnemySetUp enemyConfig;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] Transform[] wayPointArray;

    int index;
    Vector3 targetLastPos;

    void Start()
    {
        moveSpeed = agent.speed;
        enemyConfig.idleTimer = 0f;
        enemyConfig.patrolTimer = 0f;
        enemyConfig.targetTransform = GameObject.FindGameObjectWithTag(enemyConfig.targetTag).GetComponent<Transform>();
    }


    void Update()
    {
        if (enemyConfig.health <= 0)
        {
            Death();
            enemyConfig.health = 0;
        }

        if (GlobalBool.isGameOver || GlobalBool.isPaused) return;

        switch (state)
        {
            case EnemyState.IDLE: IdleBehaviour(); break;
            case EnemyState.PATROL: PatrolBehaviour(); break;
            case EnemyState.CHASE: ChaseBehaviour(); break;
            case EnemyState.ATTACK: AttackBehaviour(); break;
        }

        if (IsTargetInChaseRange())
        {
            state = EnemyState.CHASE;


        }
        if (IsInAtkRange()) state = EnemyState.ATTACK;

    }

    #region Bool Functions

    bool IsTargetInChaseRange() // is target within range of enemy?
    {
        if (Vector3.Distance(enemyConfig.targetTransform.position, transform.position) < enemyConfig.detectionRange) // if target is within range.
        {
            return true;
        }

        if (state == EnemyState.CHASE && Vector3.Distance(wayPointArray[index].position, transform.position) > enemyConfig.maxChaseDistance) // if target is out of max range from patrol pos after chasing, return back to patrol state. 
                                                                                                                                             //can change to player instead of patrol pos for more chase.
        {
            state = EnemyState.PATROL;
            agent.isStopped = false; // false == AI can move, true == cannot move.
            return false;
        }

        return false;
    }

    bool IsInAtkRange()
    {
        if (Vector3.Distance(enemyConfig.targetTransform.position, transform.position) <= enemyConfig.attackRange)
        {
            print("target in range");
            agent.isStopped = true;
            return true;
        }
        else
        {
            enemyAnimator.SetBool("isAttacking", false);
            print("target no longer in range");
            agent.isStopped = false;
            return false;
        }


    }

    #endregion

    public void IdleBehaviour()
    {
        enemyConfig.idleTimer += Time.deltaTime;
        agent.speed = moveSpeed;
        

        if (enemyConfig.idleTimer >= enemyConfig.idleDuration)
        {
            state = EnemyState.PATROL;
            enemyAnimator.SetBool("isPatrol", true);
            hasReachedDestination = false;
            enemyConfig.idleTimer = 0f;
        }
        else
        {
            enemyAnimator.SetBool("isPatrol", false);
            enemyAnimator.SetBool("isChasing", false);
        }

    }

    public void PatrolBehaviour() // patrol around set points. return to this state when target leaves range.
    {

        enemyAnimator.SetBool("isChasing", false);

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
            state = EnemyState.IDLE;
            enemyAnimator.SetBool("isPatrol", false);
        }
        else enemyAnimator.SetBool("isPatrol", true);

    }

    public void ChaseBehaviour()
    {
        agent.speed = chaseSpeed;
        print("Chasing" + enemyConfig.targetTag);

        Vector3 targetCurrentPos = enemyConfig.targetTransform.position;

        if (targetCurrentPos != targetLastPos)
        {
            // CHASE LOGIC
            agent.isStopped = false;
            agent.SetDestination(enemyConfig.targetTransform.position); // chase target
            enemyAnimator.SetBool("isPatrol", false);
            enemyAnimator.SetBool("isChasing", true);
        }
        else // AI stops when player stops running around.
        {
            targetLastPos = targetCurrentPos;
            agent.isStopped = true; 
            enemyAnimator.SetBool("isPatrol", false);
            enemyAnimator.SetBool("isChasing", false);
        }

    }

    public void AttackBehaviour()
    {
        print("Time to attack!");

        enemyAnimator.SetBool("isAttacking", true); // need to set this bool false somehow.

        // ATTACK LOGIC HERE

        // TODO: stop enemy from moving when attacking. [DONE]
        // TODO: enemy to chase player after attacking and when still in range. [DONE]
        // TODO: attack and damage player.
        // TODO: stop enemy from moving completely when attack anim is playing.
        // TODO: add a decal indicator to show attack up if any.
    }

    public void StunnedBehaviour()
    {
        // STUN LOGIC HERE
    }

    #region TakeDamage & Death Functions

    public void TakeDamage(float damageAmount) // when enemy takes damage
    {
        enemyConfig.health -= damageAmount;

    }

    public void Death()
    {
        enemyConfig.isKilled = true;

        // add death anim, vfx, sounds here
    }

    #endregion
}
