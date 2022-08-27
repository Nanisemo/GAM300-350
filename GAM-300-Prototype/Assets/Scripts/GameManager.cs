using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Animator gameOverCanvas;

    void Start()
    {
        gameOverCanvas = GameObject.Find("Game Over Canvas").GetComponent<Animator>();
    }

    void Update()
    {
        DisplayGameOverUI();
    }

    void DisplayGameOverUI()
    {
        if (GlobalBool.isGameOver) gameOverCanvas.SetTrigger("GameOver");
    }
}
