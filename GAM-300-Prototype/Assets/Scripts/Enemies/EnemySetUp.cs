using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Setup", menuName = "ScriptableObject/Enemy Setup")]
// to hold the base stats and variables for an enemy.
public class EnemySetUp : ScriptableObject
{
    public string enemyName;

    public string targetTag;
    public Transform targetTransform;

    public float health;
    public float damage;

    public float idleTimer;
    public float idleDuration;

    public float patrolTimer;
    public float patrolDuration;

    public float detectionRange;
    public float attackRange;

    public bool isKilled;
    public bool isStunned;

    // add NavMesh Configs here if needed.

    private void Awake()
    {
        targetTransform = GameObject.FindGameObjectWithTag(targetTag).GetComponent<Transform>();
    }


}
