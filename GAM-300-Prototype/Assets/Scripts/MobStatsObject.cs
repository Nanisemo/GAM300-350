// /*!
//  * file:		MobStats.cs
//  * author:	Neo Ting Wei Terrence
//  * email:	neo.w@digipen.edu
//  * project:	UnityPrototype
//  * brief:	TODO
//  *
//  * Copyright © 2021 DigiPen, All rights reserved.
//  */

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mob Stats", menuName = "Mob Stats", order = 0)]
public class MobStatsObject : ScriptableObject
{
    public StatsSystem.MobStats stats;
}