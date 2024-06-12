using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemyManager : MonoSingletonGeneric<EnemyManager>
{

    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private int enemiesLayerNumber;

    [SerializeField]
    private LayerMask enemiesLayerMask;

    [SerializeField]
    private Transform[] spawnPositions;

    [SerializeField]
    private Transform player;

    private float zombiesSpawningInterval;
    private float skeletonsSpawningInterval;
    private PlayerMovement playerTarget;
    private int currentLevel;


    private List<Enemy> zombieList = new List<Enemy>();
    private List<Enemy> skeletonList = new List<Enemy>();
    private EnemyPoolService enemyPoolService;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        EventService.Instance.LevelEnd += OnLevelEnd;
        EventService.Instance.LevelUP += OnLevelUp;
        enemyPoolService = transform.GetComponent<EnemyPoolService>();
        playerTarget = GameObject.FindObjectOfType<PlayerMovement>();
        OnGameStart();
    }

    private void OnGameStart()
    {
        currentLevel = 1;

        SetEnemiesSpawningIntervals();
        StartSpawingEnemies();
    }

    private void OnLevelEnd()
    {
        CancelInvoke();
        DisableAllEnemies();
    }
    private void OnLevelUp()
    {
        currentLevel = PlayerManager.Instance.GetCurrentLevel();
        if (currentLevel == 4)
        {
            zombiesSpawningInterval /= 2f;
            skeletonsSpawningInterval /= 2f;
        }

        if (currentLevel == 7)
        {
            zombiesSpawningInterval /= 3f;
            skeletonsSpawningInterval /= 3f;
        }
        StartSpawingEnemies();
    }

    private void SetEnemiesSpawningIntervals()
    {
        zombiesSpawningInterval = enemies[0].transform.GetComponent<Enemy>().GetSpawiningInterval();
        skeletonsSpawningInterval = enemies[1].transform.GetComponent<Enemy>().GetSpawiningInterval();
    }

    private void StartSpawingEnemies()
    {
        //InvokeRepeating(nameof(SpawnAsteroids), 2, spawiningTimeDifference);
        SetEnemiesLayerMask();
        StartSpawningZombies();
        StartSpawningSkeletons();
    }

    private void SetEnemiesLayerMask()
    {
        enemiesLayerMask = 1 << enemiesLayerNumber;
    }

    private void StartSpawningZombies()
    {
        InvokeRepeating(nameof(SpawnZombies), 2, zombiesSpawningInterval);
    }

    private void StartSpawningSkeletons()
    {
        if (currentLevel >= 3)
            InvokeRepeating(nameof(Spawnskeletons), skeletonsSpawningInterval, skeletonsSpawningInterval);
    }

    // Getting zombie enemy or Creating enemy from the pooling service

    private void SpawnZombies()
    {
        Enemy enemy = enemyPoolService.GetEnemy(ObjectType.Zombie, enemies[0]);

        enemy.transform.position = GetSpawner().position;
        enemy.gameObject.SetActive(true);

        zombieList.Add(enemy);
    }

    // Getting skeleton enemy or Creating enemy from the pooling service

    private void Spawnskeletons()
    {
        Enemy enemy = enemyPoolService.GetEnemy(ObjectType.Skeleton, enemies[1]);

        enemy.transform.position = GetSpawner().position;
        enemy.gameObject.SetActive(true);
        skeletonList.Add(enemy);
    }

    private Transform GetSpawner()
    {
        int spawnIndex = 0;
        spawnIndex = UnityEngine.Random.Range(0, spawnPositions.Length);
        return spawnPositions[spawnIndex];
    }

    public LayerMask GetEnemiesLayerMask()
    {
        return enemiesLayerMask;
    }

    public Enemy CreateEnemy(ObjectType enemyType)
    {
        int enemyIndex = (int)enemyType;

        GameObject _enemy = Instantiate(enemies[enemyIndex], GetSpawner().position, GetSpawner().rotation);
        return _enemy.GetComponent<Enemy>();
    }

    //Disabling all the enemies from the scene and returning to the object pool
    private void DisableAllEnemies()
    {
        for (int i = 0; i < zombieList.Count; i++)
        {
            if (zombieList[i] != null)
            {
                zombieList[i].gameObject.SetActive(false);
                enemyPoolService.ReturnItem(zombieList[i]);
                zombieList[i] = null;
            }
        }

        for (int i = 0; i < skeletonList.Count; i++)
        {
            if (skeletonList[i] != null)
            {
                skeletonList[i].gameObject.SetActive(false);
                enemyPoolService.ReturnItem(skeletonList[i]);
                skeletonList[i] = null;
            }
        }
    }

    public void DestroyEnemy(Enemy _enemy)
    {

        if (_enemy.GetEnemyType() == ObjectType.Zombie)
        {
            for (int i = 0; i < zombieList.Count; i++)
            {
                if (_enemy == zombieList[i])
                {
                    _enemy.gameObject.SetActive(false);
                    enemyPoolService.ReturnItem(_enemy);
                    zombieList[i] = null;
                }
            }
        }
        else if (_enemy.GetEnemyType() == ObjectType.Skeleton)
        {
            for (int i = 0; i < skeletonList.Count; i++)
            {
                if (_enemy == skeletonList[i])
                {
                    _enemy.gameObject.SetActive(false);
                    enemyPoolService.ReturnItem(_enemy);
                    skeletonList[i] = null;
                }
            }
        }
    }
}
