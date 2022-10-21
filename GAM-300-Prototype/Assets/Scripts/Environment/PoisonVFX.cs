using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PoisonVFX : MonoBehaviour
{
    public Volume poisonVolume;
    public Animator poisonVolumeAnim;
    int minWeight;

    void Start()
    {
        poisonVolume.weight = minWeight;
    }

    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            poisonVolumeAnim.SetTrigger("MaxWeight"); // when entering the poison zone.
         //   poisonVolume.profile.TryGet<LensDistortion>

        }
    }

    private void OnTriggerExit(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            poisonVolumeAnim.SetTrigger("MinWeight"); // when leaving the poison zone.
        }
    }
}
