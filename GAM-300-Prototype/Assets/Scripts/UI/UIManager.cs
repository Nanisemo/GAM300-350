using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gui;

    private void Update()
    {
        if (GlobalBool.isGameOver)
            gui.SetActive(false);
        else
            gui.SetActive(true);
    }
}
