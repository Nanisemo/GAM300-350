using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private bool
        isCooldown = false;

    public float
        cooldownTime = 5.0f,
        cooldownTimer = 0.0f;

    [SerializeField]
    private Image
        abilityCooldown;

    [SerializeField]
    private TMP_Text ability_cooldownText;

    private void Start()
    {
        ability_cooldownText.gameObject.SetActive(false);

        abilityCooldown.fillAmount = 0.0f;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Debug.Log("User pressed Q");
            isCooldown = true;
            cooldownTimer = cooldownTime;
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0.0f)
            {
                isCooldown = false;
                cooldownTimer = 0.0f;
                ability_cooldownText.gameObject.SetActive(false);
                abilityCooldown.fillAmount = 0.0f;
            } 
            else {
                abilityCooldown.fillAmount = cooldownTimer / cooldownTime;
            }
        }
    }

    public void UseSpell()
    {
        if (isCooldown)
        {
            Debug.Log("user cast an ability");
        }
        else
        {
            isCooldown = true;
            ability_cooldownText.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
        }
    }
}
