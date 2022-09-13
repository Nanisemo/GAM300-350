using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LowHPShaderController : MonoBehaviour
{
    [SerializeField] float duration = 5;
    [SerializeField] float fadeInStep = 100f; // snapping values.

    [SerializeField] Material LowHPMat;
    [SerializeField] RenderFeatureToggler featToggle;

    float currentFadeValue;
    float dangerousFadeAmount = 30f;
    float lowerFadeOutAmount = 60f;
    float maxFadeOutAmount = 1000f;
    float minFadeInAmount = 20f;

    bool lowerToHigher; // is hp going from low to high

    PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        LowHPMat.SetFloat("_Intensity", maxFadeOutAmount);
        currentFadeValue = maxFadeOutAmount;
        lowerToHigher = false;
    }

    void Update()
    {
        switch (playerController.currentHealth)
        {
            case 0: FullWarning(); break;
            case 1: FullWarning(); break;
            case 2: FullWarning(); break;
            case 3: MidWarning(); break;
            case 4: LowWarning(); break;
            case 5: NoWarning(); break;
        }
    }

    void FullWarning() // when HP <= 2.
    {
        switch (lowerToHigher)
        {
            case true: currentFadeValue -= Time.unscaledDeltaTime * duration; break;
            case false: currentFadeValue -= Time.unscaledDeltaTime * duration * fadeInStep; break; // minor issue of the shader snapping when transitioning from 3 to 2.
                // if step is removed and when HP immediately drops to 2, the fadevalue will get screwed over.
        }

        if (currentFadeValue <= minFadeInAmount)
        {
            currentFadeValue = minFadeInAmount;
            lowerToHigher = true;
        }

        LowHPMat.SetFloat("_Intensity", currentFadeValue);

    }

    void NoWarning() // Completely fades out the shader when HP == 5.
    {
        currentFadeValue += Time.unscaledDeltaTime * duration * fadeInStep;
        if (currentFadeValue >= maxFadeOutAmount) currentFadeValue = maxFadeOutAmount;
        LowHPMat.SetFloat("_Intensity", currentFadeValue);
        lowerToHigher = false;

        if (currentFadeValue == maxFadeOutAmount) featToggle.DisableFeature();
    }

    void LowWarning() // when HP == 4
    {
        switch (lowerToHigher)
        {
            case false:
                featToggle.EnableFeature();
                currentFadeValue -= Time.unscaledDeltaTime * duration * fadeInStep;
                if (currentFadeValue <= lowerFadeOutAmount) currentFadeValue = lowerFadeOutAmount; break;
            case true:
                currentFadeValue += Time.unscaledDeltaTime * duration;
                if (currentFadeValue >= lowerFadeOutAmount) currentFadeValue = lowerFadeOutAmount; break;
        }

        LowHPMat.SetFloat("_Intensity", currentFadeValue);
    }

    void MidWarning() // when HP == 3
    {
        switch (lowerToHigher)
        {
            case false:
                currentFadeValue -= Time.unscaledDeltaTime * 10;
                if (currentFadeValue <= dangerousFadeAmount) currentFadeValue = dangerousFadeAmount; break;
            case true:
                currentFadeValue += Time.unscaledDeltaTime * 10;
                if (currentFadeValue >= dangerousFadeAmount) currentFadeValue = dangerousFadeAmount; break;
        }

        LowHPMat.SetFloat("_Intensity", currentFadeValue);
    }

    public IEnumerator DisableMaterial()
    {
        yield return new WaitForSecondsRealtime(4f);
        currentFadeValue += Time.unscaledDeltaTime * duration * fadeInStep;
    }
}
