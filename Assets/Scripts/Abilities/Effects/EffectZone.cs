using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectZone : MonoBehaviour {

    public enum EffectZoneDuration {
        Instant = 0,
        Persistant = 1,
    }



    public string spawnEffect;
    public string impactEffect;

    public LayerMask LayerMask { get; protected set; }

    protected Effect parentEffect;
    protected List<GameObject> targets = new List<GameObject>();


    public virtual void Initialize(Effect parentEffect, LayerMask mask)
    {
        this.parentEffect = parentEffect;
        this.LayerMask = mask;
    }

    protected abstract void Apply(GameObject target);
    protected abstract void Remove(GameObject target);

    protected virtual void ApplyToAllTargets()
    {
        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            Apply(targets[i]);
        }
    }

    protected virtual void RemoveAllTargets()
    {
        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            Remove(targets[i]);
        }
    }

    protected virtual void CleanUp()
    {
        Destroy(gameObject);
    }

    protected virtual bool CheckHitTargets(GameObject target)
    {
        if (target == null)
            return false;

        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            if (targets[i] == target)
                return false;
        }

        targets.AddUnique(target);
        return true;
    }

    protected GameObject IsProjectileInTargets(Projectile projectile)
    {
        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            if (targets[i].Projectile() == null)
                continue;

            if (targets[i].Projectile() == projectile)
                return targets[i];
        }

        return null;
    }

    protected GameObject IsEntityInTargets(Entity entity)
    {
        int count = targets.Count;
        for (int i = 0; i < count; i++)
        {
            if (targets[i].Entity() == null)
                continue;

            if (targets[i].Entity() == entity)
                return targets[i];
        }

        return null;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {

    }

    protected virtual void OnTriggerExit(Collider other)
    {

    }

    protected virtual void OnTriggerStay(Collider other)
    {

    }




}
