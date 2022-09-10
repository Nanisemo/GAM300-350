using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerController playerController;

    public float
        player_CurrentHealth,
        player_maxHealth;

    public Image[] hearts;

    public Sprite
        fullHearts,
        emptyHearts,
        halfHearts;

    private void Update()
    {
        // NOTE: Rmb to link currentHealth and maxhealth to the Player Controller
        //player_CurrentHealth = GameObject.Find("Player").GetComponent("PlayerController").currentHealth;
        //player_maxHealth = GameObject.Find("Player").GetComponent("PlayerController").maxHealth;

        HealthUI();
    }

    void HealthUI()
    {
        if (player_CurrentHealth > player_maxHealth)
            player_CurrentHealth = player_maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player_CurrentHealth)
                hearts[i].sprite = fullHearts;
            else
                hearts[i].sprite = emptyHearts;

            if (i < player_maxHealth)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }
}
