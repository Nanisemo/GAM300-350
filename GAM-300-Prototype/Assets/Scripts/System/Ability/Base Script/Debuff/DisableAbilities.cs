using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Debuff/Disable Abilities")]
public class DisableAbilities : Ability
{
    private GameObject abilityCooldown;
    private AbilityCooldown abilityCooldown1;

    public override void Initialize(GameObject obj)
    {
        abilityCooldown = GameObject.FindGameObjectWithTag("AbilityUI");
        abilityCooldown1 = abilityCooldown.GetComponent<AbilityCooldown>();
    }

    public override void TriggerAbility()
    {
        disableAbilities();
    }

    public override void DeactivateAbility()
    {
        enableAbilities();
    }

    public void disableAbilities()
    {
        abilityCooldown1.enabled = false;
    }
    public void enableAbilities()
    {
        abilityCooldown1.enabled = true;
    }
}
