using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusStatAdjustment : Status {

    public float baseAdjustmentvalue;
    public BaseStat.StatType targetStat;
    public StatModifier.StatModificationType modType;
    public bool permanent;
    public StatCollection.StatModifierOption[] options;

    protected List<StatModifier> mods = new List<StatModifier>();
    protected float adjValue;


    public StatusStatAdjustment(StatusInfo info, StatAdjustmentInfo adjInfo) : base(info) 
    {
        StatAdjSetup(adjInfo);
    }

    //public StatusStatAdjustment(StatusInfo info, StatAdjustmentInfo adjInfo, float duration, float interval ) : base (info, duration, interval)
    //{
    //    StatAdjSetup(adjInfo);
    //}

    private void StatAdjSetup(StatAdjustmentInfo adjInfo)
    {
        this.baseAdjustmentvalue = adjInfo.adjustmentValue;
        this.adjValue = adjInfo.adjustmentValue;
        this.targetStat = adjInfo.targetStat;
        this.modType = adjInfo.modType;
        this.permanent = adjInfo.permanent;
        this.options = adjInfo.options.ToArray();

        //mod = new StatModifier(adjValue, modType);

        Tick();
    }

    private void CreateAndApplyStatMod()
    {
        if (permanent == false)
        {
            StatModifier mod = new StatModifier(adjValue, modType);
            mods.Add(mod);

            StatAdjustmentManager.ApplyTrackedStatMod(SourceEffect.ParentAbility.Source.GetStats(), Target.GetStats(), targetStat, mod, options);
        }
        else
        {
            StatAdjustmentManager.ApplyUntrackedStatMod(SourceEffect.ParentAbility.Source.GetStats(), Target.GetStats(), targetStat, adjValue, modType, options);
        }
    }

    protected override void Tick()
    {
        CreateAndApplyStatMod();
    }


    public override void Stack()
    {
        base.Stack();
        adjValue += baseAdjustmentvalue;

    }

    protected override void CleanUp()
    {
        base.CleanUp();

        if (permanent == false)
        {
            int count = mods.Count;
            for (int i = 0; i < count; i++)
            {
                StatAdjustmentManager.RemoveTrackedStatMod(SourceEffect.ParentAbility.Source.GetStats(), Target.GetStats(), targetStat, mods[i], options);
            }
        }

    }


}
