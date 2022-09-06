using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCheck : MonoBehaviour
{
    public ComboData comboData;

    private List<string> inputQueue;

    private float timingWindow, timingWindowDefault;
    private int maxQueueCount;
    private bool isTimerRunning;

    private void Awake()
    {
        inputQueue          = new List<string>();
        timingWindowDefault = 0.25f;
        timingWindow        = timingWindowDefault;
        maxQueueCount       = 3;
    }
    public void AddToQueue(string x)
    {
        if (inputQueue.Count < maxQueueCount)
        {
            if (!isTimerRunning)
            {
                inputQueue.Add(x);
                isTimerRunning = true;
            }
            else
            {
                if (timingWindow <= 0)
                {
                    ClearQueue();
                }
                else
                {
                    inputQueue.Add(x);
                    timingWindow = timingWindowDefault;
                }
            }
        }
        else
        {
            CheckQueue();
        }
    }
    private void ClearQueue()
    {
        inputQueue.Clear();
        isTimerRunning = false;
        timingWindow = timingWindowDefault;
    }
    private void CheckQueue() //THONKING HOW TO NOT HARDCODE THIS PART & ALSO USE TOE JSON FILE DATA (TMR BAH)
    {
        /*switch (inputQueue.Count)
        {
            case 1:
                if (comboData.slot1[0] == inputQueue[0])
                {
                    print("Executed slot 1");
                }
                break;
            case 2:
                if (comboData.slot2[0] == inputQueue[0] && comboData.slot2[1] == inputQueue[1])
                {
                    print("Executed slot 2");
                }
                else if (comboData.slot3[0] == inputQueue[0] && comboData.slot3[1] == inputQueue[1])
                {
                    print("Executed slot 3");
                }
                break;
            case 3:
                if (comboData.slot4[0] == inputQueue[0] && comboData.slot4[1] == inputQueue[1] && comboData.slot4[2] == inputQueue[2])
                {
                    print("Executed slot 4");
                }
                break;
        }*/
        ClearQueue();
    }
    public void RunTimer()
    {
        timingWindow -= Time.deltaTime;
    }
    private void Update()
    {
        if (isTimerRunning)
        {
            RunTimer();
            if (timingWindow < 0)
            {
                CheckQueue();
            }
        }
    }
}
