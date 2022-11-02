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

    List<RangeEnemyState> rangedEnemyState;
    List<RangeEnemyState> currRangedEnemyState;

    private void Start()
    {
        GetEnemyStates();
        GetRangedEnemyStates();
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

    void GetRangedEnemyStates()
    {
        rangedEnemyState = new List<RangeEnemyState>();
        currRangedEnemyState = new List<RangeEnemyState>();

        //gets all initial states of enemies
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Ranged Enemy").Length; i++)
        {
            rangedEnemyState.Add(GameObject.FindGameObjectsWithTag("Ranged Enemy")[i].GetComponent<RangedEnemy>().state);
            currRangedEnemyState.Add(GameObject.FindGameObjectsWithTag("Ranged Enemy")[i].GetComponent<RangedEnemy>().state);
        }
    }
    private void CheckForCombat()
    {
        for (int i = 0; i < enemyState.Count; i++)
        {
            currEnemyState[i] = GameObject.FindGameObjectsWithTag("Enemy")[i].GetComponent<Enemy>().state;
        }

        for (int i = 0; i < rangedEnemyState.Count; i++)
        {
            currRangedEnemyState[i] = GameObject.FindGameObjectsWithTag("Ranged Enemy")[i].GetComponent<RangedEnemy>().state;
        }

        if ((currEnemyState.Contains(EnemyState.ATTACK) || currEnemyState.Contains(EnemyState.CHASE))|| currRangedEnemyState.Contains(RangeEnemyState.ATTACK))
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
