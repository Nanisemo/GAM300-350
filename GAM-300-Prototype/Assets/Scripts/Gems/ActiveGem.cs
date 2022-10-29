using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gems/Active Gem")]
public class ActiveGem : Gem
{
    public Ability gemAbility;
    public AbilityTypes abilityTypes = AbilityTypes.active;
}
