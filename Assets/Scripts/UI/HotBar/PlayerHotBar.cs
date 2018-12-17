using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHotBar : MonoBehaviour
{

    public List<PlayerAbilitySlot> slots = new List<PlayerAbilitySlot>();


    public void SetQuickBarSlot(Ability ability, int slot)
    {
        if(slot > slots.Count)
        {
            Debug.LogError("Index Out of Range " + slot + " is more than " + slots.Count);
            return;
        }

        slots[slot].SetAbility(ability);
    }


}
