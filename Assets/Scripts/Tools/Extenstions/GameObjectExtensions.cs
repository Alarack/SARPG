﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions {



    public static StatCollection GetStats(this GameObject go)
    {
        StatCollection result = null;

        Entity entity = go.Entity();
        if (entity != null)
            result = entity.Stats;

        Projectile projectile = go.Projectile();
        if (projectile != null)
            result = projectile.ProjectileStats;

        return result;
    }

    public static Entity Entity(this GameObject go)
    {
        return go.GetComponent<Entity>();
    }

    public static Projectile Projectile(this GameObject go)
    {
        return go.GetComponent<Projectile>();
    }

    public static MonoBehaviour GetMonoBehaviour(this GameObject go)
    {
        return go.GetComponent<MonoBehaviour>();
    }

    public static bool IsSelfOrAlly(this GameObject go, GameObject test)
    {
        if (go == test)
            return true;

        if (go.layer == test.layer)
            return true;

        return false;
    }


    public static Constants.EffectSourceType GetSourceType(this GameObject go)
    {
        if (go.Entity() == null && go.Projectile() != null)
            return Constants.EffectSourceType.Projectile;

        if (go.Projectile() == null && go.Entity() != null)
            return Constants.EffectSourceType.Entity;

        return Constants.EffectSourceType.None;
    }



}
