using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gem : ScriptableObject
{
    public enum AbilityTypes
    {
        passive,
        active
    }

    public string gemName = "New Gem";

    public abstract void Initialize(GameObject obj);
    public abstract void GainAbility();

    public abstract void ActivatePassiveAbility();
}
