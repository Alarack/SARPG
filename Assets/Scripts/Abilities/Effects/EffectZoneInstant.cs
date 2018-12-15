using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectZoneInstant : EffectZone {



    public override void Initialize(Effect parentEffect, LayerMask mask)
    {
        base.Initialize(parentEffect, mask);

        Invoke("CleanUp", 0.5f);
    }


    protected override void OnTriggerStay(Collider other)
    {
        if (LayerTools.IsLayerInMask(LayerMask, other.gameObject.layer) == false)
            return;


        Apply(other.gameObject);

    }


    protected override void Apply(GameObject target)
    {
        if (CheckHitTargets(target) == false)
            return;

        parentEffect.Apply(target);
    }


    protected override void Remove(GameObject target)
    {
        //parentEffect.Remove(target.gameObject);
        targets.RemoveIfContains(target);
    }







}
