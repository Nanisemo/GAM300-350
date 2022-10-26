using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularBullet : MonoBehaviour
{
    public GameObject hitImpactPrefab;
    public EnemySetUp enemyConfig;

    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo != null)
        {
            HitImpact();
        }

        //if (hitInfo.CompareTag("Player"))
        //{
        //    PlayerController player = hitInfo.gameObject.GetComponent<PlayerController>();
        //    player.TakeDamage(enemyConfig.damage);
        //}
    }

    void HitImpact()
    {
        // instantiate VFX
        Instantiate(hitImpactPrefab, transform.position, transform.rotation);

        // destroy self
        Destroy(gameObject);
    }
}
