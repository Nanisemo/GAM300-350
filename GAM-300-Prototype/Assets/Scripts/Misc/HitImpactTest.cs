using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitImpactTest : MonoBehaviour
{
    public GameObject hitImpactVFX;
    public float hitStopDuration = 0.3f;

    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player Hitbox"))
        {
            FindObjectOfType<CameraShake>().ShakeCamera();
            FindObjectOfType<HitStop>().StartHitStop(hitStopDuration);
            
            Vector3 hitPointPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(hitImpactVFX, hitPointPos, Quaternion.identity);
        }
    }
}
