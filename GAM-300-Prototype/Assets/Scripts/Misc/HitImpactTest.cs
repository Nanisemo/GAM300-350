using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitImpactTest : MonoBehaviour
{
    public GameObject hitImpactVFX;

    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player Hitbox"))
        {
            Vector3 hitPointPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(hitImpactVFX, hitPointPos, Quaternion.identity);
        }
    }
}
