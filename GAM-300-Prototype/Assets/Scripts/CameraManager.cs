/*
CameraManager

PLACE ALL CODE NECESSARY TO IMPLEMENT
YOUR CAMERA IN THIS SCRIPT.

DO NOT MODIFY ANY OTHER SCRIPTS.
*/

/*
    LIST OF IMPLEMENTED FEATURES:
    - Follows player
    - Locks to features of the game world
        - walls, horizontal and vertical boundaries
        - platforms
    - scrolls smoothly 
*/

//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    public GameObject playerGO;
    private Camera mainCamera;

    public float defaultLerpSpeed = 1.0f;
    public float maxSpeed;

    public float zoomIn = 1.0f;
    public float zoomOut = 1.0f;
    public float zoomSpeed = 1.0f;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        //1 setting transform allows camera to follow player object
        Vector3 desiredPos = Vector3.Lerp(transform.position, playerGO.transform.position, Time.unscaledDeltaTime * defaultLerpSpeed);
        if ((transform.position - desiredPos).magnitude > maxSpeed) desiredPos = transform.position + (playerGO.transform.position - transform.position).normalized * maxSpeed;

        transform.position = desiredPos;

        if (GlobalBool.isInCombat)
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomIn, Time.fixedUnscaledDeltaTime * zoomSpeed);
        else
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomOut, Time.fixedUnscaledDeltaTime * zoomSpeed);

    }


} //end class
