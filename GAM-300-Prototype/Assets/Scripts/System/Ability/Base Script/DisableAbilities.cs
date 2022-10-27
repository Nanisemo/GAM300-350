using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Debuff/Disable Abilities")]
public class DisableAbilities : Ability
{
    private AbilityList abilityList;

    private PassiveGem passiveGem;

    public GameObject abilityCooldown;
    public AbilityCooldown abilityCooldown1;

    private bool isInZone;

    public override void Initialize(GameObject obj)
    {
        passiveGem = obj.GetComponent<PassiveGem>();
        abilityCooldown = GameObject.FindGameObjectWithTag("AbilityUI");
        abilityCooldown1 = abilityCooldown.GetComponent<AbilityCooldown>();
        Debug.Log(abilityCooldown);
        Debug.Log(abilityCooldown1);
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
