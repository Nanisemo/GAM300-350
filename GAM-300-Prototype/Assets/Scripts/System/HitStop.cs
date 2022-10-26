using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    #region Hit Stop Stuffs

    bool waiting;
    public float hitStopDuration = 0.1f;

    #endregion

    public void StartHitStop(float duration)
    {
        if (waiting) return;
        Time.timeScale = 0;
        StartCoroutine(ResetHitStop(duration));
    }

    IEnumerator ResetHitStop(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        waiting = false;
    }
}
