using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject projectileBlast;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ObjectType objectType;

    private float damage;

    private Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Enemy>())
        {
            //Check if the enemy has isDamagable
            //then apply the damage by calling the Take Damage Method and then destroy the projectile


            IDamagable idamagable = collision.transform.GetComponent<IDamagable>();
            if (idamagable != null)
            {
                DestroyProjectile();
                idamagable.TakeDamage(damage);
            }
            //animator.SetTrigger("enemyhit");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyUnusedProjectile());
    }

    public ObjectType GetObjectType()
    {
        return objectType;
    }
    public void DestroyProjectile()
    {
        rb.velocity = Vector2.zero;
        projectileBlast.SetActive(true);
        //transform.GetComponent<SpriteRenderer>().enabled = false;
        //Destroy(gameObject, 0.3f);
        StartCoroutine(DelayInDestroyProjectile());
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    IEnumerator DelayInDestroyProjectile()
    {
        yield return new WaitForSeconds(0.3f);
        projectileBlast.SetActive(false);
        ProjectileSpawner.Instance.DestroyProjectile(this);
    }

    IEnumerator DestroyUnusedProjectile()
    {
        yield return new WaitForSeconds(5f);
        ProjectileSpawner.Instance.DestroyProjectile(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
