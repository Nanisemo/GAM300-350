using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    DEFAULT,
    COMBAT
}

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public Transform orientation;
    public Transform playerModel;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;
    public CameraState currentState;

    public GameObject defaultCamera;
    public GameObject combatCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        if (currentState == CameraState.DEFAULT) PlayerRotation();
        else if (currentState == CameraState.COMBAT) CombatDirection();

        if (GlobalBool.isInCombat) ChangeCameraState(CameraState.COMBAT); else ChangeCameraState(CameraState.DEFAULT);

    }

    void PlayerRotation()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = orientation.forward * vertical + orientation.right * horizontal;

        if (inputDirection.magnitude != 0)
        {
            playerModel.forward = Vector3.Slerp(playerModel.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    void CombatDirection()
    {
        Vector3 combatDirection = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
        orientation.forward = combatDirection.normalized;

        playerModel.forward = combatDirection.normalized;
    }

    public void ChangeCameraState(CameraState newState)
    { 

        if (newState == CameraState.DEFAULT)
        {
            defaultCamera.SetActive(true);
            combatCamera.SetActive(false);
        }
        if (newState == CameraState.COMBAT)
        {
            defaultCamera.SetActive(false);
            combatCamera.SetActive(true);
        }

        currentState = newState;
    }
}
