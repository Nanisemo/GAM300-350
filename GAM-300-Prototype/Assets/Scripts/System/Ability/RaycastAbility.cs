using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// use this as example when creating a type of ability.
// For exmaple: one for buffs and all
[CreateAssetMenu(menuName = "Abilities/ProjectileAbility")]
public class RaycastAbility : Ability
{
    
    public int gunDamage = 1;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Color laserColor = Color.white;

    private RaycastShootTriggerable rcShoot;

    public override void Initialize(GameObject obj)
    {
        rcShoot = obj.GetComponent<RaycastShootTriggerable>();
        rcShoot.Initialize();

        rcShoot.gunDamage = gunDamage;
        rcShoot.weaponRange = weaponRange;
        rcShoot.hitForce = hitForce;
        rcShoot.laserLine.material = new Material(Shader.Find("Unlit/Color"));
        rcShoot.laserLine.material.color = laserColor;
    }

    public override void TriggerAbility()
    {
        rcShoot.Fire();
    }
}
