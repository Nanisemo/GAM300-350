using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightGradient : MonoBehaviour
{
   // [SerializeField] float x, y;
    public Gradient myGradient;
    public float strobeDuration = 2f;

    public Light light;

    //bool isScrolling;


    void Update()
    {
        float t = Mathf.PingPong(Time.unscaledTime / strobeDuration, 1f);
        light.color = myGradient.Evaluate(t);

        //if (!isScrolling)
        //{
        //    x = Random.Range(-0.01f, 0.01f);
        //    y = Random.Range(-0.01f, 0.01f);
        //    StartCoroutine(ResetScrollValue());
        //}
    }

    //IEnumerator ResetScrollValue()
    //{
    //    isScrolling = true;
    //    print("changing scroll value right now");
    //    yield return new WaitForSecondsRealtime(5f);
    //    isScrolling = false;
    //}
}
