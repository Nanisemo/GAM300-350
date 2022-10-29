// /*!
//  * file:		EnemyYeet.cs
//  * author:	Neo Ting Wei Terrence
//  * email:	neo.w@digipen.edu
//  * project:	UnityPrototype
//  * brief:	TODO
//  *
//  * Copyright © 2021 DigiPen, All rights reserved.
//  */

using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class EnemyYeet : MonoBehaviour
    {
        [SerializeField]
        int weight = 1;
        [SerializeField]
        float duration = 1;
        
        NavMeshAgent agent;
        Rigidbody rb;

        //TODO: Use Enemy isStunned
        //Enemy enemy;

        float t;
        Vector3 targetPosition;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (agent.enabled) return; //Don't run as we're not being shoved.

            if (t >= 1)
            {
                rb.isKinematic = true;
                agent.enabled = true;
                return;
            }
            
            t += Time.deltaTime / duration;
        }

        void FixedUpdate()
        {
            if (agent.enabled) return; //Don't run as we're not being shoved.
            
            rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, t));
        }
        
        public void Push(Vector3 force)
        {
            agent.enabled = false;
            t = 0;
            rb.isKinematic = false;

            targetPosition = transform.position + Vector3.ProjectOnPlane(force, Vector3.up) * (1.0f / weight);
            Debug.Log("YEET");
        }
    }
}