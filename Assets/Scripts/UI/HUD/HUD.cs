using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance;
    public PlayerHotBar hotBar;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }



    public static void SetQuickBarSlot(Ability ability, int slotIndex)
    {
        Instance.hotBar.SetQuickBarSlot(ability, slotIndex);
    }

    


}
