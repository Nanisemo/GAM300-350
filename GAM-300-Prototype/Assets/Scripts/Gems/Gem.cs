using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public enum AbilityTypes
    {
        passive,
        active
    }

    public Ability 
        _passive,
        _active;

    private bool
        isInteracted;

    public AbilityTypes type;

    private GameObject abilityUI;

    private void Start()
    {
        abilityUI = GameObject.FindGameObjectWithTag("AbilityUI");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            abilityUI.GetComponent<AbilityCooldown>().ability = _active;
        }
    }
}
