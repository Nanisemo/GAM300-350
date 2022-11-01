using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    PlayerController pc;
    PlayerMove pm;
    Animator anim;
    TimeSystem ts;
    public HitStop hitStop;

    [Header("Variables")]
    int attackCount;
    public bool hasDodgeIFrame;
    public bool hasAbilityIFrame;
    public bool damageTakenIFrameActive;
    public bool isBusy;

    public float damageIFrameDuration = 0.3f;

    public float firstHitStop = 0.05f;
    public float secondHitStop = 0.1f;
    public float thirdHitStop = 0.13f;

    [Header("Effects")]

    public GameObject aoeHitPrefab;
    public GameObject healVFXPrefab;
    public Transform spawnPoint;
    public Transform healPoint;


    private void Start()
    {
        ts = GameObject.FindGameObjectWithTag("GM").GetComponent<TimeSystem>();
        pc = GetComponentInParent<PlayerController>();
        pm = GetComponentInParent<PlayerMove>();
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
        else if (ts.isActive)
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        if (pm.isGrounded) Punch();
        else return;
    }
    private void Punch()
    {
        if (Input.GetMouseButtonDown(0) && !pc.isAttacking && !isBusy)
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
        hasDodgeIFrame = true;
    }

    public void DeActivateIFrames()
    {
        hasDodgeIFrame = false;
    }


    public void SpawnHealEffect()
    {
        Instantiate(healVFXPrefab, healPoint.position, healVFXPrefab.transform.rotation);
    }

    #region HitStop

    void FirstHitStop()
    {
        hitStop.hitStopDuration = firstHitStop;
    }

    void SecondHitStop()
    {
        hitStop.hitStopDuration = secondHitStop;
    }

    void ThirdHitStop()
    {
        hitStop.hitStopDuration = thirdHitStop;
    }

    #endregion

    void SetIsBusy()
    {
        isBusy = true;
    }

    void ResetIsBusy()
    {
        isBusy = false;
    }

    void RenderSkinMesh()
    {
        if (!pm.meshTrailRenderer.isTrailActive)
        {
            pm.meshTrailRenderer.isTrailActive = true;
            StartCoroutine(pm.meshTrailRenderer.RenderMeshTrail(.1f));
        }

    }

    void SpawnAoeParticles()
    {
        Vector3 offsetPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y - 1.4f, spawnPoint.position.z);
        Instantiate(aoeHitPrefab, offsetPosition, aoeHitPrefab.transform.rotation);
    }


    public void ActivateAbilityIFrames()
    {
        hasAbilityIFrame = true;
    }

    public void DeActivateAbilityIFrames()
    {
        hasAbilityIFrame = false;
    }

    #endregion

    private bool AnimPlaying(Animator anim, string animName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
            return true;
        else
            return false;
    }

    public void SetDamageIFrame()
    {
        damageTakenIFrameActive = true;
        StartCoroutine(ResetDamageIFrame());
    }

    IEnumerator ResetDamageIFrame()
    {
        yield return new WaitForSeconds(damageIFrameDuration);
        damageTakenIFrameActive = false;
    }
}
