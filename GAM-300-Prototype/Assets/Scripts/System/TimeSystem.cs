using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public GameObject timeSlowVolume;

    #region Time Slow Stuffs

    public float timeFractureDuration = 3f; // for how long?
    public float timeSlowFactor = 0.05f; //by how slow?

    float normalTimeScale = 1f; // use this variable to revert time back to normal.

    public float timeFractureCoolDown = 10f; // CD for how long?
    public float coolDownTimer = 0f;

    bool isOnCoolDown;
    bool isActive;

    bool toStartNormalisingTime;
    bool toStartCoolDown;




    #endregion

    void Start()
    {

    }


    void Update()
    {
        if (toStartNormalisingTime) RevertTime();
        if (toStartCoolDown) StartCountDown();
    }

    public void TimeFracture()
    {
        if (!isActive && !isOnCoolDown) SlowingTime();
    }

    IEnumerator ExitFracture()
    {
        yield return new WaitForSecondsRealtime(timeFractureDuration); // might clash with UI menu stuff bc of timescaling.
        toStartNormalisingTime = true;
        print("time fracture stopped, going on cooldown");
    }

    void SlowingTime()
    {
        print("!");
        isActive = true;
        timeSlowVolume.SetActive(true);
        Time.timeScale = timeSlowFactor; // slows down time by 20 times.
        Time.fixedDeltaTime = Time.timeScale * 0.2f;

        StartCoroutine(ExitFracture());
    }

    void RevertTime()
    {
        Time.timeScale += (1f / timeFractureDuration) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, normalTimeScale); // timescale cannot go below 0 and beyond 1.

        if (Time.timeScale > 0.98f)
        {
            isActive = false;
            timeSlowVolume.SetActive(false);
            print("time fracture ended!");
            Time.timeScale = normalTimeScale;
            toStartCoolDown = true;
            toStartNormalisingTime = false;
            isOnCoolDown = true;
            coolDownTimer = timeFractureCoolDown;
        }
    }

    void StartCountDown()
    {
        if (coolDownTimer > 0f)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if (isOnCoolDown && coolDownTimer <= 0.1f)
        {
            isOnCoolDown = false;
            toStartCoolDown = false;
            print("off cooldown, can trigger again");
        }

    }
}
