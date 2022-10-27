using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gems/Passive Gem")]
public class PassiveGem : Gem
{
    public bool isInZone;

    public Ability gemAbility;

    public AbilityTypes abilityTypes = AbilityTypes.passive;

    public override void Initialize(GameObject obj)
    {
        
    }

    public override void GainAbility()
    {
        // Do Nothing
    }

    public override void ActivatePassiveAbility()
    {
        if(isInZone) {
            // Activate Ability
        }
    }
}
