using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    public CinemachineFreeLook defaultCamera;
    public CinemachineFreeLook combatCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if (GlobalBool.isGameOver || GlobalBool.isPaused || GlobalBool.isLoading) return;

        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        if (currentState == CameraState.DEFAULT) PlayerRotation();
        //else if (currentState == CameraState.COMBAT) CombatDirection();

       // if (GlobalBool.isInCombat) ChangeCameraState(CameraState.COMBAT); else ChangeCameraState(CameraState.DEFAULT);

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
            defaultCamera.Priority = 10;
            combatCamera.Priority = 0;
        }
        //if (newState == CameraState.COMBAT)
        //{
        //    defaultCamera.Priority = 0;
        //    combatCamera.Priority = 10;
        //}

        currentState = newState;
    }
}
