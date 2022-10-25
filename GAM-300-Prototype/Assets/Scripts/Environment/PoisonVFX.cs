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

    LensDistortion ld;
    public float minLensDistortion = 0.4f;
    public float distortionStep = 0.1f;
    public float maxLensDistortion = 0.8f;
    public float lerpTimeOffset = 1f; // higher value = slower lerp.
    bool isPoisoned;

    void Start()
    {
        poisonVolume.weight = minWeight;
        poisonVolume.profile.TryGet<LensDistortion>(out ld);

    }

    void Update()
    {
        if (!isPoisoned)
        {
            ld.intensity.value = minLensDistortion;
        }
        else
        {
            float t = Time.deltaTime / lerpTimeOffset;
            float currentValue = ld.intensity.value;
            float newValue = currentValue + distortionStep;

            ld.intensity.value = Mathf.Lerp(currentValue, newValue, t);

            if (ld.intensity.value > maxLensDistortion)
            {
                ld.intensity.value = maxLensDistortion;
            }
        }
    }

    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            poisonVolumeAnim.SetTrigger("MaxWeight"); // when entering the poison zone.
        }
    }

    private void OnTriggerStay(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player") /*&& !hasOVerstayed*/)
        {
            print("innnnn poison");
            isPoisoned = true;

        }

    }

    private void OnTriggerExit(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            isPoisoned = false;
            poisonVolumeAnim.SetTrigger("MinWeight"); // when leaving the poison zone.
        }
    }
}
