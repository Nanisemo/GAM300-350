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
                meshRend.material = trailMaterial;

                Destroy(tempObj, trailLifeTime);

            }


            yield return new WaitForSeconds(dashTime);
        }

        isTrailActive = false;
    }

    IEnumerator FadeMeshTransparency()
    {
        yield return null;
    }
}
