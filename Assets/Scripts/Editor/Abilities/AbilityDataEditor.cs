using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbilityData))]
public class AbilityDataEditor : Editor {

    private AbilityData _abilityData;


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        _abilityData = (AbilityData)target;

        EditorGUILayout.LabelField("Ability Info", EditorStyles.boldLabel);

        _abilityData.abilityName = EditorGUILayout.TextField("Ability Name", _abilityData.abilityName);
        _abilityData.abilityIcon = EditorHelper.ObjectField<Sprite>("Icon", _abilityData.abilityIcon);
        _abilityData.useDuration = EditorHelper.FloatField("Use Time", _abilityData.useDuration);
        _abilityData.overrideOtherAbilities = EditorGUILayout.Toggle("Interupt Other Abilities?", _abilityData.overrideOtherAbilities);
        _abilityData.procChance = EditorHelper.PercentFloatField("Proc Chance", _abilityData.procChance);

        EditorGUILayout.Separator();
        //_abilityData.triggers = EditorHelper.DrawList("Activation Methods", _abilityData.triggers, true, Constants.AbilityActivationMethod.None, true, DrawAbilityTriggers);

        EditorGUILayout.LabelField("Activation Triggers", EditorStyles.boldLabel);

        _abilityData.activations = EditorHelper.DrawExtendedList(_abilityData.activations, "Activation", DrawActivator);


        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);

        _abilityData.effectData = EditorHelper.DrawExtendedList(_abilityData.effectData, "Effect", DrawEffect);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Recovery", EditorStyles.boldLabel);
        EditorGUILayout.Separator();

        _abilityData.recoveryData = EditorHelper.DrawExtendedList("Recovery Types", _abilityData.recoveryData, "Recovery", DrawRecoveryData);


        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }



    private AbilityActivationInfo DrawActivator(AbilityActivationInfo entry)
    {
        entry.activationConditions = EditorHelper.DrawList("Activation Options", "Option", entry.activationConditions, true, Constants.AbilityActivationCondition.Normal, true, DrawListOfEnums);
        entry.activationMethod = EditorHelper.EnumPopup("Activation Method", entry.activationMethod);

        switch (entry.activationMethod)
        {
            case Constants.AbilityActivationMethod.Timed:
                entry.activationTime = EditorGUILayout.FloatField("Time", entry.activationTime);
                break;

            case Constants.AbilityActivationMethod.StatChanged:
                entry.targetstat = EditorHelper.EnumPopup("What Stat Changed?", entry.targetstat);
                entry.gainedOrLost = EditorHelper.EnumPopup("Gained or Lost?", entry.gainedOrLost);
                entry.statChangeTarget = EditorHelper.EnumPopup("Target of Stat Change", entry.statChangeTarget);
                break;
        }

        return entry;
    }

    private EffectData DrawEffect(EffectData entry)
    {
        EditorGUILayout.LabelField("Basic Effect Info", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        entry.effectName = EditorGUILayout.TextField("Effect Name", entry.effectName);
        entry.animationTrigger = EditorGUILayout.TextField("Anim Trigger", entry.animationTrigger);
        entry.layerMask = EditorHelper.LayerMaskField("Layer Mask", entry.layerMask);
        entry.effectType = EditorHelper.EnumPopup("Effect Type", entry.effectType);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Delivery", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        entry.deliveryMethod = EditorHelper.EnumPopup(entry.deliveryMethod);
        EditorGUILayout.Separator();
        switch (entry.deliveryMethod)
        {
            case Constants.EffectDeliveryMethod.Projectile:
                entry.projectileInfo = DrawProjectileInfo(entry.projectileInfo);
                break;

            case Constants.EffectDeliveryMethod.Rider:
                entry.riderTarget = EditorGUILayout.TextField("Target Effect Name", entry.riderTarget);
                break;
        }
        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Zone Info", EditorStyles.boldLabel);
        entry.effectOrigin = EditorHelper.EnumPopup("Origin Location", entry.effectOrigin);
        entry.effectZoneInfo = DrawZoneInfo(entry.effectZoneInfo);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Duration", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        entry.durationType = EditorHelper.EnumPopup(entry.durationType);

        switch (entry.durationType)
        {
            case Constants.EffectDurationType.Duration:
            case Constants.EffectDurationType.Periodic:
                entry.statusTypeInfo = DrawStatusTypeInfo(entry.statusTypeInfo);
                break;
        }
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Specific Effect", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        switch (entry.effectType)
        {
            case Constants.EffectType.StatAdjustment:
                entry.adjInfo = DrawStatAdjustmentInfo(entry.adjInfo);
                break;
        }

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Tags", EditorStyles.boldLabel);
        EditorGUILayout.Separator();
        entry.tags = EditorHelper.DrawList("Tags", "Tag", entry.tags, true, Constants.EffectTag.None, true, DrawListOfEnums);

        return entry;
    }


    private ZoneInfo DrawZoneInfo(ZoneInfo entry)
    {
        entry.durationType = EditorHelper.EnumPopup("Zone Duration Type", entry.durationType);

        if(entry.durationType == EffectZone.EffectZoneDuration.Persistant)
        {
            EditorGUILayout.Separator();
            entry.duration = EditorHelper.FloatField("Duration (0 = INF)", entry.duration);
            entry.interval = EditorHelper.FloatField("Interval (0 = None)", entry.interval);
            entry.removeEffectOnExit = EditorGUILayout.Toggle("Remove On Exit?", entry.removeEffectOnExit);
            EditorGUILayout.Separator();
        }

        entry.size = EditorHelper.EnumPopup("Size", entry.size);
        entry.shape = EditorHelper.EnumPopup("Shape", entry.shape);


        return entry;
    }

    private StatusTypeInfo DrawStatusTypeInfo(StatusTypeInfo entry)
    {
        entry.statusType = EditorHelper.EnumPopup("Status Type", entry.statusType);
        entry.stackMethod = EditorHelper.EnumPopup("Stack Method", entry.stackMethod);

        if(entry.stackMethod == Constants.EffectStackingMethod.LimitedStacks)
            entry.maxStacks = EditorGUILayout.IntField("Max Stacks", entry.maxStacks);

        EditorGUILayout.Separator();
        entry.duration = EditorHelper.FloatField("Duration (0 = INF)", entry.duration);
        entry.interval = EditorHelper.FloatField("Interval (0 = None)", entry.interval);

        EditorGUILayout.Separator();
        entry.onCompleteEffectName = EditorGUILayout.TextField("OnComplete Effect", entry.onCompleteEffectName);


        return entry;
    }

    private StatAdjustmentInfo DrawStatAdjustmentInfo(StatAdjustmentInfo entry)
    {
        entry.targetStat = EditorHelper.EnumPopup("Target Stat", entry.targetStat);
        entry.modType = EditorHelper.EnumPopup("Mod Type", entry.modType);
        entry.adjustmentValue = EditorHelper.FloatField("Value", entry.adjustmentValue);
        entry.permanent = EditorGUILayout.Toggle("Permanent? (Damage / Healing)", entry.permanent);
        entry.options = EditorHelper.DrawList("Base or Cap?", "Option", entry.options, true, StatCollection.StatModifierOption.Base, true, DrawListOfEnums);


        return entry;
    }

    private ProjectileInfo DrawProjectileInfo(ProjectileInfo entry)
    {
        entry.spreadType = EditorHelper.EnumPopup("Spread Type", entry.spreadType);
        entry.prefabName = EditorGUILayout.TextField("Prefab Name", entry.prefabName);
        entry.error = EditorHelper.FloatField("Error Range", entry.error);
        entry.projectileCount = EditorGUILayout.IntField("How Many?", entry.projectileCount);

        if (entry.projectileCount > 1)
            entry.burstDelay = EditorHelper.FloatField("Burst Delay", entry.burstDelay);

        return entry;
    }

    private RecoveryData DrawRecoveryData(RecoveryData entry)
    {
        entry.type = EditorHelper.EnumPopup("Recovery Type", entry.type);

        switch (entry.type)
        {
            case Constants.AbilityRecoveryType.Cooldown:
                entry.cooldown = EditorHelper.FloatField("Cooldown", entry.cooldown);
                break;

            case Constants.AbilityRecoveryType.Kills:
                entry.kills = EditorGUILayout.IntField("Kills", entry.kills);
                break;
        }

        return entry;
    }












    private Constants.AbilityActivationMethod DrawAbilityTriggers(List<Constants.AbilityActivationMethod> list, int index)
    {
        Constants.AbilityActivationMethod result = EditorHelper.EnumPopup("ActivationMethod", list[index]);
        return result;
    }

    private Constants.AbilityActivationCondition DrawActivationConditions(List<Constants.AbilityActivationCondition> list, int index)
    {
        Constants.AbilityActivationCondition result = EditorHelper.EnumPopup("Condition", list[index]);
        return result;
    }


    private T DrawListOfEnums<T>(List<T> list, int index, string label) where T : struct, System.IFormattable, System.IConvertible
    {
        T result = EditorHelper.EnumPopup(label, list[index]);

        return result;
    }

}
