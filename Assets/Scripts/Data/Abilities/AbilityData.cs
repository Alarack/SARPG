﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectTag = Constants.EffectTag;
using AbilityActivationMethod = Constants.AbilityActivationMethod;

[CreateAssetMenu(menuName = "Ability Data")]
[System.Serializable]
public class AbilityData : ScriptableObject {

    public string abilityName;
    //public List<AbilityActivationMethod> triggers = new List<AbilityActivationMethod>();
    public List<AbilityActivationInfo> activations = new List<AbilityActivationInfo>();

    public Sprite abilityIcon;
    public float useDuration;
    public float procChance = 1f;
    public bool overrideOtherAbilities;

    public List<EffectData> effectData = new List<EffectData>();
    public List<RecoveryData> recoveryData = new List<RecoveryData>();

    //public List<EffectTag> tags = new List<EffectTag>();

}
