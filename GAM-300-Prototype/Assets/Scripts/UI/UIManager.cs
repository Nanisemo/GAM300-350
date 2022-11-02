using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gui;

    private void Update()
    {
        if (GlobalBool.isGameOver)
            gui.SetActive(false);
        else
            gui.SetActive(true);


        if (Input.GetKeyDown(KeyCode.Escape)) QuitGame();

        if (Input.GetKeyDown(KeyCode.T)) ReloadScene();

    }


    public void QuitGame()
    {
        Application.Quit();
        print("quit");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
