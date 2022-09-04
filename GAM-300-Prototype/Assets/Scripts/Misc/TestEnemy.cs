using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public Animator animcController;
    Transform player;

    float timer = 0f;
    float targetedTimer = 5f;

    void Start()
    {
        animcController = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= targetedTimer)
        {
            animcController.SetBool("isPatrol", true);
            timer = 0f;
        }
        else animcController.SetBool("isPatrol", false);

        if (Vector3.Distance(player.position, transform.position) < 8)
        {
            animcController.SetBool("isChasing", true);
        }
        else animcController.SetBool("isChasing", false);
    }
}
