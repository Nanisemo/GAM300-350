using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOver;

    void Start()
    {
        gameOver.SetActive(false);

        GlobalBool.isGameOver = false;
        GlobalBool.isPaused = false;
        GlobalBool.isInCombat = false;
    }

    void Update()
    {
        if (GlobalBool.isGameOver) DisplayGameOverUI();

        // if there is at least 1 enemy in combat, set isInCombat to be true
        GlobalBool.isInCombat = (GlobalBool.enemiesInCombat.Count > 0);
    }

    void DisplayGameOverUI()
    {
        gameOver.SetActive(true);
        LowHPShaderController matController = GetComponent<LowHPShaderController>();
        StartCoroutine(matController.DisableMaterial());

    }


}
