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

        //gets all initial states of enemies
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
        {
            enemyState.Add(GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().state);
            currEnemyState.Add(GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().state);
        }
    }
    private void CheckForCombat()
    {
        for (int i = 0; i < enemyState.Count; i++)
        {
            currEnemyState[i] = GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().state;
        }
        if (currEnemyState.Contains(EnemyState.ATTACK) || currEnemyState.Contains(EnemyState.CHASE))
        {
            if (inCombat != true)
            {
                inCombat = true;
                UpdateBGM();
            }
        }
        else
        {
            if (inCombat != false)
            {
                inCombat = false;
                UpdateBGM();
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
