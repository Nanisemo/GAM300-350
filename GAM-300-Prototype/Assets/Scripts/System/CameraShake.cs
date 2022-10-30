using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineImpulseSource source;

    void Start()
    {
        source = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera()
    {
        print("shaking");
        source.GenerateImpulse(Camera.main.transform.forward);
    }
}
