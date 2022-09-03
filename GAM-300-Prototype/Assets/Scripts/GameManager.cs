using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Animator gameOverCanvas;
    public GameObject gameOver;

    void Start()
    {
        gameOver.SetActive(false);
    }

    void Update()
    {
        if (GlobalBool.isGameOver) DisplayGameOverUI();
    }

    void DisplayGameOverUI()
    {
        gameOver.SetActive(true);
        LowHPShaderController matController = GetComponent<LowHPShaderController>();
        StartCoroutine(matController.DisableMaterial());

    }
}
