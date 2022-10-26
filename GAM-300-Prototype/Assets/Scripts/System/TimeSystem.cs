using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeSystem : MonoBehaviour
{
    // public GameObject timeSlowVolume;
    public int physicsFrameRate = 200;

    private float physicsDeltaTime;

    #region Time Slow Stuffs

    public float timeFractureDuration = 3f; // for how long?
    public float timeSlowFactor = 0.05f; //by how slow?

    float normalTimeScale = 1f; // use this variable to revert time back to normal.

    public float timeFractureCoolDown = 5f; // CD for how long?
    public float coolDownTimer = 0f;

    bool isOnCoolDown;
    public bool isActive;

    bool toStartNormalisingTime;
    bool toStartCoolDown;

    #endregion

    #region Time Slow Volume

    public Volume timeSlowVolume;
    public Animator volAnim;

    float minWeight = 0;
    #endregion

    void Awake()
    {
        physicsDeltaTime = 1 / (float)physicsFrameRate;
    }

    private void Start()
    {
        timeSlowVolume.weight = minWeight;
    }

    void Update()
    {
        if (GlobalBool.isPaused) volAnim.updateMode = AnimatorUpdateMode.Normal; else volAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        if (toStartNormalisingTime) RevertTime();
        if (toStartCoolDown) StartCountDown();

    }

    #region Time Slow Function
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

        volAnim.SetTrigger("MaxWeight");

        Time.timeScale = timeSlowFactor; // slows down time by 20 times.
        Time.fixedDeltaTime = Time.timeScale * physicsDeltaTime;

        StartCoroutine(ExitFracture());
    }

    void RevertTime()
    {
        Time.timeScale += (1f / timeFractureDuration) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, normalTimeScale); // timescale cannot go below 0 and beyond 1.

        if (Time.timeScale > 0.98f)
        {
            isActive = false;

            volAnim.SetTrigger("MinWeight");

            print("time fracture ended!");
            Time.timeScale = normalTimeScale;
            Time.fixedDeltaTime = physicsDeltaTime;
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

    #endregion

   
}
