using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolService : PoolingService<Enemy>
{
    private ObjectType enemyType;
    private GameObject enemyPrefab;

   public Enemy GetEnemy(ObjectType enemyType, GameObject enemyPrefab)
    {
        this.enemyType = enemyType;
        this.enemyPrefab = enemyPrefab;
        return GetItem(enemyType);
    }

    protected override Enemy CreateItem()
    {
        GameObject enemy = GameObject.Instantiate(enemyPrefab);

        return enemy.GetComponent<Enemy>();
    }
}
