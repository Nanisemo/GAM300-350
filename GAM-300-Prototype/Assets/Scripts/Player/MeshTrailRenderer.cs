using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrailRenderer : MonoBehaviour
{
    public float meshRefreshRate = 0.1f;
    public float trailLifeTime = 0.2f;
    public bool isTrailActive;
    public Transform player;

    public Material trailMaterial;
    public string shaderVariableRef;
    public float shaderVariableRate = 0.1f;
    public float shaderRefreshRate = 0.05f;


    SkinnedMeshRenderer[] meshArray;
    public IEnumerator RenderMeshTrail(float dashTime)
    {
        while (dashTime > 0)
        {
            dashTime -= meshRefreshRate;

            if (meshArray == null) meshArray = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < meshArray.Length; i++)
            {
                GameObject tempObj = new GameObject();
                tempObj.transform.SetPositionAndRotation(player.transform.position, player.transform.rotation);
                MeshRenderer meshRend = tempObj.AddComponent<MeshRenderer>();
                MeshFilter meshFilter = tempObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                meshArray[i].BakeMesh(mesh);
                meshFilter.mesh = mesh;
                meshRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                meshRend.material = trailMaterial;
                trailMaterial.SetFloat(shaderVariableRef, 1);

                StartCoroutine(FadeMeshTransparency(trailMaterial, 0, shaderVariableRate, shaderRefreshRate));

                Destroy(tempObj, trailLifeTime);

            }

            yield return new WaitForSecondsRealtime(dashTime);
        }

        isTrailActive = false;
    }

    IEnumerator FadeMeshTransparency(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVariableRef);
        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVariableRef, valueToAnimate);
            yield return new WaitForSecondsRealtime(refreshRate);
        }

    }
}
