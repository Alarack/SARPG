using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier  {

    public enum StatModificationType {
        Additive,
        Multiplicative
    }

    public float Value { get; private set; }
    public StatModificationType ModType { get; private set; }

    public StatModifier(float value, StatModificationType modType) {
        this.Value = value;
        this.ModType = ModType;
    }

    public float GetValueByModType(StatModificationType type) {
        return type == ModType ? Value : 0f;
    }

}
