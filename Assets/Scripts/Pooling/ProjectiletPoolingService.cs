using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectiletPoolingService : PoolingService<WandProjectile>
{
    private ObjectType objectType;
    private GameObject wandProjectilePrefab;

    public WandProjectile GetWandProjectile(ObjectType objectType, GameObject wandProjectilePrefab)
    {
        this.objectType = objectType;
        this.wandProjectilePrefab = wandProjectilePrefab;
        return GetItem(objectType);
    }

    protected override WandProjectile CreateItem()
    {
        GameObject enemy = GameObject.Instantiate(wandProjectilePrefab);

        return enemy.GetComponent<WandProjectile>();
    }
}
