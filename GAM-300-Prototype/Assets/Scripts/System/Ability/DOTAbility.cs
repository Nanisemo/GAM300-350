using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTAbility : MonoBehaviour
{
    [SerializeField] private float damageOverTime;
    [SerializeField] private float damageInterval;
    private PlayerController playerController;
    [SerializeField] private bool isInZone;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = true;
            StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = false;
        }
    }

    IEnumerator DamageOverTime()
    {
        while (isInZone)
        {
            playerController.TakeDamage(damageOverTime);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}

