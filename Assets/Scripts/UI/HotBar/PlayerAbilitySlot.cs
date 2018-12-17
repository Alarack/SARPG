using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerAbilitySlot : MonoBehaviour, IPointerClickHandler
{
    public Image dimmer;
    public Image icon;

    public Sprite emptySlot;

    private Ability ability;

    public void SetAbility(Ability ability)
    {
        this.ability = ability;

        if (ability != null)
            icon.sprite = ability.AbilityIcon;
        else
            icon.sprite = emptySlot;
    }

    private void Update()
    {
        if (ability != null && ability.RecoveryManager.HasRecovery)
        {
            UpdateRecovery();
        }
        else if(dimmer.fillAmount != 0)
        {
            dimmer.fillAmount = 0;
        }
    }

    private void UpdateRecovery()
    {
        float ratioOfFill = Mathf.Abs(ability.RecoveryManager.Ratio - 1);
        dimmer.fillAmount = ratioOfFill;

        //Debug.Log(ratioOfFill + " is current ratio");

        if(ability.RecoveryManager.HasCharges && dimmer.fillAmount != 0)
        {
            dimmer.fillAmount = 0;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                if(ability != null)
                    ability.Activate();
                break;

            case PointerEventData.InputButton.Right:
                Debug.Log("Open ablity screen here");
                break;
        }


    }


}
