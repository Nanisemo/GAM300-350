using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/HealAbility")]
public class Heal : Ability
{
    public float healAmount;

    private PlayerController playerController;

    public override void Initialize(GameObject obj)
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); 
    }

    public override void TriggerAbility()
    {
        healingAbility(healAmount);
    }

    public void healingAbility(float healingAmt)
    {
        playerController.currentHealth += healingAmt;
    }
}
