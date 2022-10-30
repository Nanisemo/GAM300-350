// /*!
//  * file:		YeetTest.cs
//  * author:	Neo Ting Wei Terrence
//  * email:	neo.w@digipen.edu
//  * project:	UnityPrototype
//  * brief:	TODO
//  *
//  * Copyright © 2021 DigiPen, All rights reserved.
//  */

using System;
using Enemies;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class YeetTest : MonoBehaviour
{
    [SerializeField]
    float force = 1;
    [SerializeField]
    bool move = false;
    
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        var yeet = other.GetComponent<EnemyYeet>();
        if (yeet)
        {
            yeet.Push((other.transform.position - transform.position).normalized * force);
        }
    }

    void FixedUpdate()
    {
        if (move)
            rb.velocity = Vector3.back;
    }
}