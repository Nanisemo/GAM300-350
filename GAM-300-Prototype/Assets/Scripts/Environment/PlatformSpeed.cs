using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpeed : MonoBehaviour
{
    public float upSpeed = 0.5f;
    public float downSpeed = 0.5f;

    public Animator platformAnim;

    void Start()
    {
        platformAnim.SetFloat("upSpeed", upSpeed);
        platformAnim.SetFloat("downSpeed", downSpeed);
    }


}
