using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // TO USE FOR CAMERA FOLLOW + CAMERA ANIMS + CAMERA EFFECTS

    Vector3 iniPos;
    Transform mainCam;
    Transform player;

    public float lerpSmooth = 1f;

    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        iniPos = mainCam.position - player.position;
    }


    void Update()
    {
        Vector3 targetCamPos = player.position + iniPos;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, lerpSmooth * Time.deltaTime);
    }

    private void LateUpdate()
    {

    }
}
