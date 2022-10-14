// /*!
//  * file:		StatBuff.cs
//  * author:	Neo Ting Wei Terrence
//  * email:	neo.w@digipen.edu
//  * project:	UnityPrototype
//  * brief:	TODO
//  *
//  * Copyright © 2021 DigiPen, All rights reserved.
//  */

namespace System
{
    public abstract class StatBuff
    {
        /// <summary>
        /// The human-readable name of the buff.
        /// </summary>
        public string name;
        /// <summary>
        /// The default time of the buff.
        /// </summary>
        public float startTime;
        /// <summary>
        /// Remaining time left in the buff. The buff dispels immediately when this is less than or equal to 0.
        /// </summary>
        public float currentTime;
        /// <summary>
        /// Determines order of calculation with other stat buffs in ascending order. Identical priorities may produce
        /// weird math behaviour.
        /// </summary>
        public int priority;

        /// <summary>
        /// Apply changes to the mob's base stats. Do take deltaTime into account when applying calculations that are
        /// time-dependent, such as poison or regeneration, as this is called every Update.
        /// </summary>
        /// <param name="baseStats">The base stats of the mob.</param>
        public virtual void ApplyStats(ref StatsSystem.MobStats baseStats) { }

        /// <summary>
        /// Perform an action when a buff wears off. Example: Shield exploding upon expiring.
        /// </summary>
        /// <remarks>Stat changes from ApplyStats should not be reverted here.</remarks>
        /// <param name="stats">The current stats of the mob.</param>
        public virtual void Dispel(ref StatsSystem.MobStats stats) { }

        /// <summary>
        /// Refreshes the buff duration, override this to add features such as stacking. 
        /// </summary>
        public virtual void Refresh()
        {
            currentTime = startTime;
        }
    }
}