using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource
        BGM_Default,
        BGM_Combat;

    private bool
        inCombat,
        combatChange;

    private List<EnemyState> enemyState;
    private List<EnemyState> currEnemyState;

    private void Start()
    {
        GetEnemyStates();
    }
    private void Update()
    {
        CheckForCombat();
    }
    private void GetEnemyStates()
    {
        enemyState = new List<EnemyState>();
        currEnemyState = new List<EnemyState>();
        //sets the enemy states to all be idle
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
        {
            enemyState.Add(GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().state);
            currEnemyState.Add(GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().state);
        }
    }
    private void CheckForCombat()
    {
        //Updates all enemy states
        for (int i = 0; i < enemyState.Count; i++)
        {
            currEnemyState[i] = GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().state;
        }
        
        for (int i = 0; i < currEnemyState.Count; i++)
        {
            //If any enemy's state has changed, run the following
            if (currEnemyState[i] != enemyState[i])
            {
                combatChange = inCombat;
                switch (currEnemyState[i])
                {
                    case EnemyState.CHASE:
                        inCombat = true;
                        break;
                    case EnemyState.ATTACK:
                        inCombat = true;
                        break;
                    default:
                        inCombat = false;
                        break;
                }
                if (inCombat != combatChange)
                    UpdateBGM();
                enemyState[i] = currEnemyState[i];
            }
        }
    }
    public void UpdateBGM()
    {
        if (inCombat)
        {
            BGM_Combat.enabled = true;
            BGM_Combat.Play();
            BGM_Default.enabled = false;
        }
        else
        {
            BGM_Combat.enabled = false;
            BGM_Default.enabled = true;
            BGM_Default.Play();
        }
    }
}
