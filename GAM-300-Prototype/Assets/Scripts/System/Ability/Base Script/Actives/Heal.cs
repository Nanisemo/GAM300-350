using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Skills/Heal")]
public class Heal : Ability
{
    // Start is called before the first frame update
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

    public override void DeactivateAbility()
    {

    }

    public void healingAbility(float healingAmt)
    {
        playerController.currentHealth += healingAmt;
        playerController.playerAnim.SetTrigger("Heal");
    }
}
