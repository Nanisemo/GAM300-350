using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOver;
    public float deathAnimLength = 1.5f;

    void Start()
    {
        gameOver.SetActive(false);

        GlobalBool.isGameOver = false;
        GlobalBool.isPaused = false;
        GlobalBool.isInCombat = false;
    }

    void Update()
    {
        if (GlobalBool.isGameOver) StartCoroutine(DisplayGameOverUI());

        // if there is at least 1 enemy in combat, set isInCombat to be true
        //GlobalBool.isInCombat = (GlobalBool.enemiesInCombat.Count > 0);
    }

    IEnumerator DisplayGameOverUI()
    {
        yield return new WaitForSeconds(deathAnimLength);
        gameOver.SetActive(true);
        LowHPShaderController matController = GetComponent<LowHPShaderController>();
        StartCoroutine(matController.DisableMaterial());

    }


}
