using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public refrences

    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject bg;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float health;
    [SerializeField]
    private int playerLevel;
    [SerializeField]
    private Vector2 playerSpawnPos;

    // private refrences
    private Renderer rend;
    float moveInputX, moveInputY;
    Vector2 moveInput;
    private bool facingRight = true;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        transform.position = playerSpawnPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        rend = bg.GetComponent<Renderer>();
        EventService.Instance.LevelUP += OnLevelUp;
    }

    private void FixedUpdate()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");

        float xValue = 0;
        if (moveInputX < 0)
        {
            xValue = -moveInputX;
        }
        else
        {
            xValue = moveInputX;
        }

        moveInput = new Vector2(moveInputX, moveInputY);

        // moving the background acording the movement of the player
        rend.material.mainTextureOffset = new Vector2(rend.material.mainTextureOffset.x - xValue * 0.05f*Time.deltaTime,
            rend.material.mainTextureOffset.y - moveInputY *0.05f* Time.deltaTime);

        MovePlayer(moveInput);
        if (facingRight == true && moveInputX < 0)
            Flip();
        if (facingRight == false && moveInputX > 0)
            Flip();
    }

    // Applying Damage when getting collided with enemy
    private void TakeDamage(float damageValue)
    {
        PlayerManager.Instance.UpdateHealth(damageValue);
        health -= damageValue;
    }

    // Applying Xp after collecting the gems
    private void TakeXP(float xp)
    {
        PlayerManager.Instance.UpdateXP(xp);
    }

    // Moving player 
    public void MovePlayer(Vector2 move)
    {
        if (Input.GetKey(KeyCode.LeftShift) && move.magnitude != 0)
        {
            //rb.velocity = new Vector2(move * Time.fixedDeltaTime * speed * 2, rb.velocity.y);
            rb.velocity = move * Time.fixedDeltaTime * speed * 2;
            return;
        }
        //rb.velocity = new Vector2(move * Time.fixedDeltaTime * speed, rb.velocity.y);
        rb.velocity = move * Time.fixedDeltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<Enemy>())
        {
            Enemy enemy = collision.transform.GetComponent<Enemy>();
            TakeDamage(enemy.GetDamageValue());
            enemy.PlayDeathAnimation();
        }

        if (collision.transform.GetComponent<XpCollectible>())
        {
            XpCollectible xpGem = collision.transform.GetComponent<XpCollectible>();
            TakeXP(xpGem.GetXPValue());
            xpGem.DestroyCollectible();
            //Debug.Log("collected from player");
        }
    }

    private void OnLevelUp()
    {
        //playerLevel++; 
    }

    public void SetPlayerLevel(int _level)
    {
        playerLevel = _level;
    }

    public float GetPlayerSpeed()
    {
        return speed;
    }

    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    // Flipping the player 
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public float GetHealthValue()
    {
        return health;
    }

    public void IncreaseHealth(float _health)
    {
        health += _health;
    }
}