using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float damageValue;

    [SerializeField]
    private ObjectType enemyType;

    [SerializeField]
    private float spawningInterval;
    
    [SerializeField]
    private float staticHealth;

    private float currentHealth;
    private Transform target;
    private bool isUsed;
    private bool facingRight = true;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<PlayerMovement>().transform;
        animator = transform.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        currentHealth = staticHealth;
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;
        rb.velocity = (target.position - transform.position).normalized * moveSpeed;
        if (facingRight == true && rb.velocity.x < 0)
            Flip();
        if (facingRight == false && rb.velocity.x > 0)
            Flip();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.velocity = (target.position - transform.position).normalized * moveSpeed;
    }



    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("collided");

    //    if (other.transform.CompareTag("Player"))
    //    {
    //        Debug.Log("collided with player");
    //        PlayDeathAnimation();
    //    }
    //}

    private void ReduceHealth(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            PlayDeathAnimation();
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void SetSpawningInetrval(float interval)
    {
        spawningInterval += interval; 
    }

    public void SetFollowTaraget(Transform _target)
    {
        target = _target;
    }

    public float GetSpawiningInterval()
    {
        return spawningInterval;
    }

    public ObjectType GetEnemyType()
    {
        return enemyType;
    }

    public float GetDamageValue()
    {
        return damageValue;
    }

    public void TakeDamage(float damage)
    {
        ReduceHealth(damage);
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
        CollectiblesManager.Instance.SpawnXpGem(transform.position);
        //if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        //{
        //}
        StartCoroutine(DestroyEnemyObject());
    }

    IEnumerator DestroyEnemyObject()
    {
        yield return new WaitForSeconds(0.3f);
        EnemyManager.Instance.DestroyEnemy(this);

    }
}
