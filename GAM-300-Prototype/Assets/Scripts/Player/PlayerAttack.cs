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
        pc = GetComponent<PlayerController>();
        attackCount = 1;
    }
    private void Update()
    {
        if (!AnimPlaying(pc.playerAnim, "Punch 1") && !AnimPlaying(pc.playerAnim, "Punch 2") && !AnimPlaying(pc.playerAnim, "Punch 3"))
        {
            print("not attacking");
            pc.isAttacking = false;
        }
        else
        {
            print("attacking");
            pc.isAttacking = true;
        }
        /*if (attackCount > 1)
        {
            StartCoroutine("Timer");
            attackCount = 1;
        }*/
        Punch();
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(5f);
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
                        pc.playerAnim.Play("Punch 1");
                        attackCount += 1;
                    }
                    break;
                case 2:
                    if (!AnimPlaying(pc.playerAnim, "Punch 2"))
                    {
                        pc.playerAnim.Play("Punch 2");
                        attackCount += 1;
                    }
                    break;
                case 3:
                    if (!AnimPlaying(pc.playerAnim, "Punch 3"))
                    {
                        pc.playerAnim.Play("Punch 3");
                        attackCount = 1;
                    }
                    break;
            }
        }
    }

    private bool AnimPlaying(Animator anim, string animName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
            return true;
        else
            return false;
    }
}
