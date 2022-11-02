using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GainAbility : MonoBehaviour
{
    private AbilityCooldown abilityCD;
    public bool isInZone;
    [SerializeField] private Ability ability;
    [SerializeField] GameObject UItext;

    void Start()
    {
        abilityCD = GameObject.FindGameObjectWithTag("AbilityUI").GetComponent<AbilityCooldown>();
        UItext.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInZone)
        {
            abilityCD.ability = ability;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInZone = true;
            UItext.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UItext.SetActive(false);
            isInZone = false;
        }
            
    }
}
