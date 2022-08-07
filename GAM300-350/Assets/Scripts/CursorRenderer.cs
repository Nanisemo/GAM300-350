using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorRenderer : MonoBehaviour
{
    // Cursor Sprite Render
    [SerializeField] Camera mainCam;
    [SerializeField] bool lockCursor;
    [SerializeField] bool isCameraSpace;
    public GameObject mainCursor;




    private void Start()
    {
        if (lockCursor)
        {
            Cursor.visible = false;
            mainCursor.SetActive(true);
        }

    }

    void Update()
    {
        MousePosition();
    }

    public void MousePosition()
    {
        if (!isCameraSpace)
            transform.position = Input.mousePosition;
        else
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 100.0f; //distance of the plane from the camera
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }

    }

}
