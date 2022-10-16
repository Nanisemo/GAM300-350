using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalVFX : MonoBehaviour
{
    public Transform activationPoint;
    public GameObject activationEffect;
    public ParticleSystem activatedRingEffect;
    public ParticleSystem upwardsStreamEffect;
    public float ringDelayDuration = 2f;

    bool isActivated;

    void Start()
    {
        isActivated = false;
        // activationEffect.Stop();
        activatedRingEffect.Stop();

    }
    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            Instantiate(activationEffect, activationPoint.position, activationEffect.transform.rotation);
            StartCoroutine(ActivateRingEffect());
        }
    }

    private void OnTriggerExit(Collider hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            activatedRingEffect.Stop();
            upwardsStreamEffect.Play();
        }
    }

    IEnumerator ActivateRingEffect()
    {
        upwardsStreamEffect.Stop();
        yield return new WaitForSeconds(ringDelayDuration);
        activatedRingEffect.Play();
    }
}
