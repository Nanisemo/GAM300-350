using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbilities : MonoBehaviour
{
    public Ability passiveAbility;

    private void OnTriggerEnter(Collider other)
    {
        passiveAbility.TriggerAbility();
    }

    private void OnTriggerExit(Collider other)
    {
        passiveAbility.DeactivateAbility();
    }
}
