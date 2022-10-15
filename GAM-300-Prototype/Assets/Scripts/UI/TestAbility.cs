using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/EmptyAbility")]
public class TestAbility : Ability
{
    public override void Initialize(GameObject obj)
    {
        Debug.Log("Initialize Ability from TestAbility");
    }

    public override void TriggerAbility()
    {
        Debug.Log("Trigger Ability from TestAbility");
    }
}
