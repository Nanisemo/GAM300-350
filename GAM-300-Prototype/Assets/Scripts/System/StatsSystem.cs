// /*!
//  * file:		StatsSystem.cs
//  * author:	Neo Ting Wei Terrence
//  * email:	neo.w@digipen.edu
//  * project:	UnityPrototype
//  * brief:	TODO
//  *
//  * Copyright © 2021 DigiPen, All rights reserved.
//  */

using System.Collections.Generic;
using UnityEngine;

namespace System
{
    public class StatsSystem : MonoBehaviour
    {
        [Serializable]
        public struct MobStats
        {
            public int maxHealth;
            public int currentHealth;
            public int shieldHealth;
            public int damage;
            public int moveSpeed;
            //public int unitWeight;
            public int weight;
        }

        [SerializeField]
        MobStatsObject baseStatsObject;

        MobStats currentStats;
        List<StatBuff> buffs;

        public MobStats GetBaseStats()
        {
            return baseStatsObject.stats;
        }

        public MobStats GetStats()
        {
            return currentStats;
        }
        
        void LateUpdate()
        {
            currentStats = baseStatsObject.stats;

            foreach (StatBuff buff in buffs)
            {
                buff.ApplyStats(ref currentStats);

                if (buff.currentTime <= 0)
                {
                    buff.Dispel(ref currentStats);
                }
            }

            buffs.RemoveAll(buff => buff.currentTime <= 0);
        }

        void ApplyBuff(StatBuff newBuff)
        {
            int i = buffs.FindIndex(b => b.name == newBuff.name);
            if (i >= 0)
            {
                buffs[i].Refresh();
            }
            else
            {
                buffs.Add(newBuff);
                buffs.Sort((x, y) => x.priority.CompareTo(y.priority));
            }
        }
    }
}