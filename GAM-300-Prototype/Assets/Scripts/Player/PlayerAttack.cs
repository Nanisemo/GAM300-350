using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    PlayerController pc;

    [Header("Variables")]
    int attackCount;

    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();
        attackCount = 1;
    }
    private void Update()
    {
        Punch();
    }
    private void Punch()
    {
        if (Input.GetMouseButtonDown(0) && !pc.isAttacking)
        {
            switch (attackCount)
            {
                case 1:
                    if (!AnimPlaying(pc.playerAnim, "Punch 1"))
                    {
                        pc.playerAnim.SetTrigger("Punch1");
                        attackCount += 1;
                    }
                    break;
                case 2:
                    if (!AnimPlaying(pc.playerAnim, "Punch 2"))
                    {
                        pc.playerAnim.CrossFade("Punch 2", 0.1f);
                        attackCount += 1;
                    }
                    break;
                case 3:
                    if (!AnimPlaying(pc.playerAnim, "Punch 3"))
                    {
                        pc.playerAnim.CrossFade("Punch 3", 0.1f);
                        attackCount = 1;
                    }
                    break;
            }
        }
    }

    #region Animation Events
    private void ResetAttack()
    {
        attackCount = 1;
    }
    private void Attack()
    {
        pc.isAttacking = true;
    }
    private void StopAttack()
    {
        pc.isAttacking = false;
    }
    #endregion
    private bool AnimPlaying(Animator anim, string animName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
            return true;
        else
            return false;
    }
}
