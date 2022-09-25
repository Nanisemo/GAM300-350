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

    public EnemySetUp enemyConfig;

    [SerializeField] float moveSpeed;
    [SerializeField] float chaseSpeed;
    float health;

    [SerializeField] Animator enemyAnimator;
    [SerializeField] Transform[] wayPointArray;

    int index;

    public GameObject hitImpactPrefab;

    void Awake()
    {
        enemyConfig.isKilled = false;
    }
    void Start()
    {
        // enemyConfig.isKilled = false;
        health = enemyConfig.health;
        moveSpeed = agent.speed;
        enemyConfig.idleTimer = 0f;
        enemyConfig.patrolTimer = 0f;
        enemyConfig.targetTransform = GameObject.FindGameObjectWithTag(enemyConfig.targetTag).GetComponent<Transform>();

    }


    void Update()
    {

        if (GlobalBool.isGameOver || GlobalBool.isPaused) return;

        if (!enemyConfig.isKilled)
        {
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
            return false;
        }

        return false;
    }

    bool IsInAtkRange()
    {
        if (Vector3.Distance(enemyConfig.targetTransform.position, transform.position) <= enemyConfig.attackRange)
        {
            return true;
        }
        else
        {
            enemyAnimator.SetBool("isAttacking", false);
            return false;
        }

    }

    #endregion

    public void IdleBehaviour()
    {
        enemyConfig.idleTimer += Time.deltaTime;
        agent.speed = moveSpeed;
        GlobalBool.isInCombat = false;

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
        GlobalBool.isInCombat = false;
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
        GlobalBool.isInCombat = true;
        agent.speed = chaseSpeed;
        print("Chasing" + enemyConfig.targetTag);

        // CHASE LOGIC
        agent.SetDestination(enemyConfig.targetTransform.position); // chase target endlessly until different state.
        enemyAnimator.SetBool("isPatrol", false);
        enemyAnimator.SetBool("isChasing", true);

    }

    public void AttackBehaviour()
    {
        print("Time to attack!");

        enemyAnimator.SetBool("isAttacking", true);

        // ATTACK LOGIC HERE

        // TODO: stop enemy from moving when attacking. [DONE] - anim events.
        // TODO: enemy to chase player after attacking and when still in range. [DONE]
        // TODO: attack and damage player. [DONE]
        // TODO: stop enemy from moving completely when attack anim is playing. [DONE]
        // TODO: add a decal indicator to show attack up if any.
    }

    public void StunnedBehaviour()
    {
        // STUN LOGIC HERE
    }

    IEnumerator Aim()
    {
        Quaternion lookRotation = Quaternion.LookRotation(enemyConfig.targetTransform.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void FreezePosition()
    {
        agent.isStopped = true;
    }

    public void UnFreezePosition()
    {
        agent.isStopped = false;
    }

    #region TakeDamage & Death Functions

    public void TakeDamage(float damageAmount) // when enemy takes damage
    {
        if (health - damageAmount > 0)
        {
            enemyAnimator.Play("Enemy1_Hurt");
            enemyAnimator.SetBool("isAttacking", false);
            health -= damageAmount;
            print(health);
        }
        else
        {
            Death();
            health = 0;
        }
    }

    public void Death()
    {
        state = EnemyState.DEAD;
        enemyConfig.isKilled = true;
        GlobalBool.isInCombat = false;
        agent.isStopped = true;
        enemyAnimator.SetBool("isAttacking", false);
        enemyAnimator.SetBool("isPatrol", false);
        enemyAnimator.SetBool("isChasing", false);

        // add death anim, vfx, sounds here
        enemyAnimator.Play("Enemy1_Death");


    }

    #endregion

    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player Hitbox"))
        {
            PlayerController player = hitInfo.GetComponentInParent<PlayerController>();
            Instantiate(hitImpactPrefab, hitInfo.transform.position, Quaternion.identity);
            TakeDamage(player.damage);
            print("enemy ouchie ouch");
        }
    }

}
