using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GainAbility : MonoBehaviour
{
    [SerializeField]private GameObject abilityUI;
    [SerializeField]private AbilityCooldown abilityCDI;
    [SerializeField]private bool isInZone;
    [SerializeField] private Ability ability;

    // TODO: Anunu - Check why when the player exit the bool is not set by to false
    void Start()
    {
        abilityUI = GameObject.FindGameObjectWithTag("AbilityUI");
        abilityCDI = abilityUI.GetComponent<AbilityCooldown>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && isInZone)
        {
            abilityCDI.ability = ability;
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
            isInZone = true;
    }
}
