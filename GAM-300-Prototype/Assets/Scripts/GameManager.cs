using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Animator gameOverCanvas;
    public GameObject gameOver;
    public Animator camAnim;

    void Start()
    {
        gameOver.SetActive(false);
    }

    void Update()
    {
        if (GlobalBool.isGameOver) DisplayGameOverUI();

        if (GlobalBool.isInCombat)
        {
            CameraZoomOut();
        }
        else CameraZoomIn();
    }

    void DisplayGameOverUI()
    {
        gameOver.SetActive(true);
        LowHPShaderController matController = GetComponent<LowHPShaderController>();
        StartCoroutine(matController.DisableMaterial());

    }

    void CameraZoomOut()
    {
        camAnim.SetTrigger("zoomOut");
    }

    void CameraZoomIn()
    {
        camAnim.SetTrigger("zoomIn");
    }
}
