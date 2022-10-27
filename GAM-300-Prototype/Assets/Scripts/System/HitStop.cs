using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    #region Hit Stop Stuffs

    bool waiting;
    public float hitStopDuration = 0.1f;

    #endregion

    TimeSystem ts;

    void Start()
    {
        ts = GameObject.FindGameObjectWithTag("GM").GetComponent<TimeSystem>();
    }

    public void StartHitStop()
    {
        if (waiting) return;
        Time.timeScale = 0;
        StartCoroutine(ResetHitStop(hitStopDuration));
    }

    IEnumerator ResetHitStop(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        if (!ts.isActive)
        {
            Time.timeScale = 1;
        }

        waiting = false;
    }
}
