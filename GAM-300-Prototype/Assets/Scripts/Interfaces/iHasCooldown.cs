using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iHasCooldown
{
    int Id { get; }
    float CooldownDuration { get; }
}
