using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveAbilityCooldown : MonoBehaviour
{
    public Ability ability;

    public GameObject spawnWhere;

    public float timer;

    void Start()
    {
        Initialize(ability, spawnWhere);
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Enter zone");
            ability.TriggerAbility();
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0;
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Exit zone");
            ability.DeactivateAbility();
        }
    }

    public void Initialize(Ability selectedAbility, GameObject spawnWhere)
    {
        ability = selectedAbility;
        ability.Initialize(spawnWhere);
    } 


}
