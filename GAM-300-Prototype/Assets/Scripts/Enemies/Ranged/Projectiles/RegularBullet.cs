using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularBullet : MonoBehaviour
{
    public GameObject hitImpactPrefab;
    public EnemySetUp enemyConfig;

    void Start()
    {

    }


    void Update()
    {

    }

    private void OnTriggerEnter(Collider hitInfo)
    {
        if (hitInfo != null)
        {
            print(hitInfo.gameObject.name);
            HitImpact();
        }

        if (hitInfo.CompareTag("Player"))
        {
            PlayerController player = hitInfo.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(enemyConfig.damage);
        }
    }

    void HitImpact()
    {
        // instantiate VFX
        Instantiate(hitImpactPrefab, transform.position, transform.rotation);

        // destroy self
        Destroy(gameObject);
    }
}
