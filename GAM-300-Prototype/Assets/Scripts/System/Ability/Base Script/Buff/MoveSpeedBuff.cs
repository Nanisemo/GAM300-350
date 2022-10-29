using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Buff/Movement")]
public class MoveSpeedBuff : Ability
{
    // Start is called before the first frame update
    public float multiplier;

    private PlayerMove playerController;

    public override void Initialize(GameObject obj)
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    public override void TriggerAbility()
    {
        healingAbility();
    }

    public override void DeactivateAbility()
    {
        returnToOriginal(); 
    }

    public void healingAbility()
    {
        playerController.moveSpeed *= multiplier;
    }

    public void returnToOriginal()
    {
        playerController.moveSpeed /= multiplier;
    }
}
