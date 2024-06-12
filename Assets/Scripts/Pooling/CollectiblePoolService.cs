using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePoolService : PoolingService<XpCollectible>
{
    private ObjectType objectType;
    private GameObject collectiblePrefab;

    public XpCollectible GetCollectible(ObjectType objectType, GameObject xpGemPrefab)
    {
        this.objectType = objectType;
        this.collectiblePrefab = xpGemPrefab;
        return GetItem(objectType);
    }

    protected override XpCollectible CreateItem()
    {
        GameObject enemy = GameObject.Instantiate(collectiblePrefab);
        return enemy.GetComponent<XpCollectible>();
    }
}