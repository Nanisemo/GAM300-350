using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GainAbility : MonoBehaviour
{
    private AbilityCooldown abilityCD;
    public bool isInZone;
    [SerializeField] private Ability ability;

    void Start()
    {
        abilityCD = GameObject.FindGameObjectWithTag("AbilityUI").GetComponent<AbilityCooldown>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && isInZone)
        {
            abilityCD.ability = ability;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isInZone = false;
    }
}
