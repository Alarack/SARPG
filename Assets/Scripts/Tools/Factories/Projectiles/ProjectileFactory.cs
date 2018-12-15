using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileSpreadType = ProjectileInfo.ProjectileSpreadType;

public static class ProjectileFactory {



    public static Projectile CreateProjectile(ProjectileInfo info, Constants.EffectOrigin location, GameObject source, int currentProjectile = 0)
    {
        Vector3 spawnPoint;

        if (location != Constants.EffectOrigin.MousePointer)
        {
            Transform point = source.Entity().effectDelivery.GetOriginPoint(location);
            spawnPoint = point.position;
        }
        else
        {
            spawnPoint = source.transform.position;
        }

        Projectile loadedProjectile = LoadAndSpawnProjectile(info.prefabName, spawnPoint, Quaternion.identity);

        if (loadedProjectile == null)
            return null;

        return ConfigureProjectile(info, ref loadedProjectile, currentProjectile);
    }

    private static Projectile ConfigureProjectile(ProjectileInfo info, ref Projectile projectile, int currentProjectile = 0)
    {
        switch (info.spreadType)
        {
            case ProjectileSpreadType.None:

                break;

            case ProjectileSpreadType.Random:
                float error = Random.Range(info.error, -info.error);

                projectile.transform.rotation = Quaternion.Euler(projectile.transform.rotation.x, error, projectile.transform.rotation.z);
                break;

            case ProjectileSpreadType.EvenlySpread:

                float errorRange = info.error;
                float percentOfRange = info.projectileCount / errorRange;

                float offset = currentProjectile == 0 && info.projectileCount.IsOdd() ? 0 : (currentProjectile + 1) * percentOfRange;

                projectile.transform.rotation = Quaternion.Euler(projectile.transform.rotation.x, offset, projectile.transform.rotation.z);

                break;
        }

        return projectile;
    }

    private static Projectile LoadAndSpawnProjectile(string prefabName, Vector3 spawnPoint, Quaternion spawnRotation)
    {
        Projectile loadedPrefab = Resources.Load("Projectiles/" + prefabName) as Projectile;

        if(loadedPrefab == null)
        {
            Debug.LogError("Could not load projectile");
            return null;
        }

        return SpawnProjectile(loadedPrefab, spawnPoint, spawnRotation);

    }

    private static Projectile SpawnProjectile(Projectile prefab, Vector3 spawnPoint, Quaternion spawnRotation)
    {
        return GameObject.Instantiate(prefab, spawnPoint, spawnRotation) as Projectile;
    }

}


[System.Serializable]
public struct ProjectileInfo {

    public enum ProjectileSpreadType {
        None = 0,
        Random = 1,
        EvenlySpread = 2,
    }


    public string prefabName;
    public ProjectileSpreadType spreadType;
    public int projectileCount;
    public float error;
    public float burstDelay;


    public ProjectileInfo(string prefabName, ProjectileSpreadType spreadType, int projectileCount, float error, float burstDelay)
    {
        this.prefabName = prefabName;
        this.spreadType = spreadType;
        this.projectileCount = projectileCount;
        this.error = error;
        this.burstDelay = burstDelay;

    }

}
