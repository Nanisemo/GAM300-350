using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AbilityList : MonoBehaviour
{
    private PlayerController playerController;

    public  AbilityCooldown abilityCooldown;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //if (GameObject.FindGameObjectWithTag("AbilityUI").GetComponent<AbilityCooldown>() != null)
        //{
        //    Debug.Log(GameObject.FindGameObjectWithTag("AbilityUI").GetComponent<AbilityCooldown>().name);
        //}
        //abilityCooldown = GameObject.FindGameObjectWithTag("AbilityUI").GetComponent<AbilityCooldown>();
    }
    public void movementSpeedBuff(float amt)
    {
        playerController.GetComponent<PlayerMove>().moveSpeed += amt;
    }
    public void returnToOriginalSpeed()
    {
        playerController.GetComponent<PlayerMove>().moveSpeed = 7;
    }
}