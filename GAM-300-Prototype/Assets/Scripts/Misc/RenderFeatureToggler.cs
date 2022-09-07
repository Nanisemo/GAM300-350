using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct RenderFeatureToggle
{
    public ScriptableRendererFeature feature;
    public bool isEnabled;
}

[ExecuteAlways]
public class RenderFeatureToggler : MonoBehaviour
{
    [SerializeField]
    // private List<RenderFeatureToggle> renderFeatures = new List<RenderFeatureToggle>();
    RenderFeatureToggle lowHPmat;
    [SerializeField]
    private UniversalRenderPipelineAsset pipelineAsset;

    private void Update()
    {
        // TO UNCOMMENT THIS IF NEED TO HAVE MULTIPLE FEATURES TOGGLED.

        //foreach (RenderFeatureToggle toggleObj in renderFeatures)
        //{
        //    toggleObj.feature.SetActive(toggleObj.isEnabled);
        //}
    }

    public void DisableFeature()
    {
        lowHPmat.feature.SetActive(false);
    }

    public void EnableFeature()
    {
        lowHPmat.feature.SetActive(true);
    }
}