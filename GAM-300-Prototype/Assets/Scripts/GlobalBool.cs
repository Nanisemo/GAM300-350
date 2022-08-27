using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBool : MonoBehaviour
{
    public static bool isGameOver;
    public static bool isPaused;

    void Awake()
    {
        isGameOver = false;
        isPaused = false;
    }
}
