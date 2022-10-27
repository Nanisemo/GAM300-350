using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveAbilityCooldown : MonoBehaviour
{
    public Ability ability;

    public GameObject spawnWhere;

    void Start()
    {
        Initialize(ability, spawnWhere);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            ability.TriggerAbility();
    }

    private void OnTriggerExit(Collider other)
    {
        ability.DeactivateAbility();
    }

    public void Initialize(Ability selectedAbility, GameObject spawnWhere)
    {
        ability = selectedAbility;
        ability.Initialize(spawnWhere);
    } 


}
