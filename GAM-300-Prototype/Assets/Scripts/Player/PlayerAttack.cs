using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    PlayerController pc;
    Animator anim;

    [Header("Variables")]
    int attackCount;
    public bool hasIFrame;

    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
        attackCount = 1;
    }
    private void Update()
    {
        if (GlobalBool.isLoading || GlobalBool.isGameOver || GlobalBool.isPaused)
        {
            anim.updateMode = AnimatorUpdateMode.Normal;
            return;
        }
        else anim.updateMode = AnimatorUpdateMode.UnscaledTime;

        print(pc.isAttacking);
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
                        pc.playerAnim.CrossFade("Punch 1", 0.05f);
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

    public void ActivateIFrames()
    {
        hasIFrame = true;
    }

    public void DeActivateIFrames()
    {
        hasIFrame = false;
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
