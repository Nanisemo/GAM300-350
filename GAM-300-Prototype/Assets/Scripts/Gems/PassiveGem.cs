using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gems/Passive Gem")]
public class PassiveGem : Gem
{
    public Ability gemAbility;
    public AbilityTypes abilityTypes = AbilityTypes.passive;
}
