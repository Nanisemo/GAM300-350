using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBool : MonoBehaviour
{
    public static bool isGameOver;
    public static bool isPaused;
    public static bool isInCombat;

    public static List<Enemy> enemiesInCombat = new List<Enemy>();

    void Awake()
    {
        isGameOver = false;
        isPaused = false;
        isInCombat = false;
        enemiesInCombat.Clear();
    }

    public static void SetInCombat(Enemy enemy, bool _isInCombat)
    {
        // in combat, check if enemy is already accounted for in the list
        if (_isInCombat)
        {
            // enemy is accounted for
            if (enemiesInCombat.Contains(enemy)) return;
            // account for new enemy in combat
            enemiesInCombat.Add(enemy);
        }
        else
        {
            // enemy not in combat, check if enemy is still accounted for in the list
            if (enemiesInCombat.Contains(enemy)) enemiesInCombat.Remove(enemy);
        }
    }
}
