using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProjectileSpawner : MonoSingletonGeneric<ProjectileSpawner>
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float spawningTimeDifference;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileDamage;
    [SerializeField]
    private float damageIncrementValue;
    [SerializeField]
    private float SpawnSpeedIncrementValue;
    [SerializeField]
    private float overlapCircleRadius;
    [SerializeField]
    private List<Transform> targetTransforms = new List<Transform>();
    [SerializeField]
    private Transform wandContainer;
    [SerializeField]
    private bool canThrowProjectile;

    // private refrences
    private LayerMask enemiesLayerMask;
    private List<WandProjectile> projectileList = new List<WandProjectile>();
    private ProjectiletPoolingService projectiletPoolingService;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        projectiletPoolingService = GetComponent<ProjectiletPoolingService>();
        enemiesLayerMask = EnemyManager.Instance.GetEnemiesLayerMask();
        InvokeRepeating(nameof(ThrowProjectile), 2, spawningTimeDifference);

        //event listners
        EventService.Instance.ProjectileDamageIncrement += OnProjectileDamageIncrement;
        EventService.Instance.ProjectileSpawnSpeedIncrement += OnProjectileSpawnSpeedIncrement;
        EventService.Instance.LevelEnd += OnLevelEnd;
        EventService.Instance.LevelUP += OnLevelUP;
        EventService.Instance.GameOver += OnGameOver;
    }

    //Function called on OnLevelEnd
    private void OnLevelEnd()
    {
        canThrowProjectile = false;
        CancelInvoke();
    }
    //Function called on OnLevelUp
    private void OnLevelUP()
    {
        canThrowProjectile = true;
        InvokeRepeating(nameof(ThrowProjectile), 2, spawningTimeDifference);
    }

    //Function called on OnGameOver
    private void OnGameOver()
    {
        canThrowProjectile = false;
    }

    // Starting to throw the projectile
    private void ThrowProjectile()
    {
        // if tthere no need to spawn the projectile, then return with help of boolean
        if (!canThrowProjectile)
            return;

        // getting all the colliders in the colliders array which are within the radius of Overlap Circle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapCircleRadius, enemiesLayerMask);

        float nearestDistance = float.MaxValue;
        Collider2D nearestCollider = null;

        // getting the minimun distant collider enemy object only within the range of circle
        foreach (Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCollider = collider;
            }
        }

        // if there is no collider is within the circel range then it is targeting directly to the enemy spawners to target the enemies
        if (nearestCollider != null)
        {
            //Debug.Log("Nearest object: " + nearestCollider.gameObject.name);
            ShootProjectile(nearestCollider.transform);
        }
        else
        {
            int randomIndex = Random.Range(0, targetTransforms.Count);
            ShootProjectile(targetTransforms[randomIndex]);
            //Debug.Log("No objects found");
        }
    }

    // Shotting the projectile, getting them from the projectile pool, enabling the them and setting the details and valued=s of them
    private void ShootProjectile(Transform target)
    {
        WandProjectile wandProjectile = projectiletPoolingService.GetWandProjectile(ObjectType.Projectile, projectile);
        wandProjectile.gameObject.SetActive(true);
        wandProjectile.SetDamage(projectileDamage);
        projectileList.Add(wandProjectile);
        wandProjectile.transform.position = transform.position;
        wandProjectile.transform.SetParent(wandContainer);
        wandProjectile.GetComponent<Rigidbody2D>().AddForce((target.transform.position - transform.position).normalized * projectileSpeed, ForceMode2D.Impulse);
    }

    // Destroying projectile with reference
    public void DestroyProjectile(WandProjectile _projectile)
    {
        if (_projectile.GetObjectType() == ObjectType.Projectile)
        {
            for (int i = 0; i < projectileList.Count; i++)
            {
                if (_projectile == projectileList[i])
                {
                    _projectile.gameObject.SetActive(false);
                    projectiletPoolingService.ReturnItem(_projectile);
                    projectileList[i] = null;
                }
            }
        }
    }

    // functio nto get called on PowerUp OnProjectileDamageIncrement is selected
    private void OnProjectileDamageIncrement()
    {
        projectileDamage += damageIncrementValue;
    }

    // functio nto get called on PowerUp OnProjectileSpawnSpeedIncrement is selected
    private void OnProjectileSpawnSpeedIncrement()
    {
        if (spawningTimeDifference <= 0.2f)
            return;
        spawningTimeDifference -= SpawnSpeedIncrementValue;
        Debug.Log("bullet time: " + spawningTimeDifference);

    }

    // to drawa The OverLap circle
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, overlapCircleRadius);
    }
}
