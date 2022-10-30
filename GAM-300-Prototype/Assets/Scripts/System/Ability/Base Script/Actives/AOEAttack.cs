using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Skills/AOE Attack")]
public class AOEAttack : Ability
{
    private PlayerController playerController;
    private GameObject player;

    [SerializeField] private LayerMask whatAreDestructible;

    public override void Initialize(GameObject obj)
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void TriggerAbility()
    {
        CheckForDestructibles();
        playerController.playerAnim.SetTrigger("AOE");
    }

    public override void DeactivateAbility()
    {

    }

    void CheckForDestructibles()
    {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 4f, whatAreDestructible);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Enemy>())
            {
                //// Set enemy to take damage
                //collider.GetComponent<Enemy>().damageTaken = true;
                //FindObjectOfType<CameraShake>().ShakeCamera();
                //FindObjectOfType<HitStop>().StartHitStop();
                //collider.GetComponent<Enemy>().TakeDamage(playerController.damage);

                collider.GetComponent<Enemy>().ExecuteAOE();

            }

            if (collider.GetComponent<RangedEnemy>())
            {
                // Set enemy to take damage
                //collider.GetComponent<RangedEnemy>().damageTaken = true;
                //FindObjectOfType<CameraShake>().ShakeCamera();
                //FindObjectOfType<HitStop>().StartHitStop();
                //collider.GetComponent<RangedEnemy>().TakeDamage(playerController.damage);
                collider.GetComponent<RangedEnemy>().ExecuteAOE();
            }
        }
    }
}