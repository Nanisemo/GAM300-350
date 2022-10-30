using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldown : MonoBehaviour
{
    public enum keyboardInputs
    {
        Q,
        Shift,
        None
    }

    public keyboardInputs abilityButton;
    public Image darkMask;
    public TMP_Text coolDownTextDisplay;

    public Ability ability;

    [SerializeField] private GameObject weaponHolder;

    private Image myButtonImage;
    // private AudioSource abilitySource;

    private float
        coolDownDuration,
        nextReadyTime,
        coolDownTimeLeft;

    PlayerController pc;

    void Start()
    {
        Initialize(ability, weaponHolder);
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public KeyCode CheckInputs(keyboardInputs keyboard)
    {
        switch (keyboard)
        {
            case keyboardInputs.Q:
                return KeyCode.Q;
                break;
            case keyboardInputs.Shift:
                return KeyCode.LeftShift;
                break;
            case keyboardInputs.None:
                return KeyCode.None;
                break;
            default: return KeyCode.None;
        }
    }

    public void Initialize(Ability selectedAbility, GameObject weaponHolder)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        // abilitySource = GetComponent<AudioSource>();
        myButtonImage.sprite = ability.abilitySprite;
        darkMask.sprite = ability.abilitySprite;
        coolDownDuration = ability.abilityBaseCoolDown;
        ability.Initialize(weaponHolder);
        AbilityReady();
    }

    void Update()
    {
        CheckImageSprite();

        bool coolDownComplete = (Time.time > nextReadyTime);
        if (coolDownComplete && pc.pm.state != MovementState.RUNNING) // cannot use when running for now to prevent sliding.
        {
            AbilityReady();
            if (Input.GetKeyDown(CheckInputs(abilityButton)))
            {
                ButtonTriggered();
            }
        }
        else
        {
            CoolDown();
        }
    }

    private void AbilityReady()
    {
        coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        coolDownTextDisplay.text = roundedCd.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    private void ButtonTriggered()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;
        coolDownTextDisplay.enabled = true;

        //abilitySource.clip = ability.abilitySound;
        //abilitySource.Play();
        ability.TriggerAbility();
    }

    private void CheckImageSprite()
    {
        myButtonImage = GetComponent<Image>();
        myButtonImage.sprite = ability.abilitySprite;
    }
}
