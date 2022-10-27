using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gems/Active Gem")]
public class ActiveGem : Gem
{
    public bool isInZone;

    public Ability gemAbility;

    public AbilityTypes abilityTypes = AbilityTypes.active;

    private GameObject abilityUI;

    private PlayerController playerController;

    public override void Initialize(GameObject obj)
    {
        abilityUI = GameObject.FindGameObjectWithTag("AbilityUI");
    }

    public override void GainAbility()
    {
        if(Input.GetKeyDown(KeyCode.P) && isInZone) {
            abilityUI.GetComponent<AbilityCooldown>().ability = gemAbility;
        }
    }

    public override void ActivatePassiveAbility()
    {

    }
}
