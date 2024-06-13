using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoSingletonGeneric<PlayerManager>
{
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private float healthIncreamentValue;
    [SerializeField]
    private Slider xpBar;
    [SerializeField]
    private PlayerMovement player;
    [SerializeField]
    private float maxPlayerXP;
    [SerializeField]
    private int playerLevel;
    [SerializeField]
    private int maxPlayerLevel;

    private float playerHealth;
    private float playerXP;
    private bool isMaxLevelReached;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        EventService.Instance.GameOver += OnGameOver;
        EventService.Instance.GameWin += OnGameWin;
        EventService.Instance.LevelUP += OnLevelUp;
        EventService.Instance.LevelEnd += OnLevelEnd;
        EventService.Instance.PlayerHealthIncrement += OnPlayerHealthIncreased;
        EventService.Instance.MaxLevelReached += OnMaxLevelReached;


        UpdateHealthBar();
        UpdateXPBar();
        //StartCoroutine(FullHealthBar());
    }

    IEnumerator FullHealthBar()
    {
        yield return new WaitForSeconds(2f);
        healthBar.maxValue = playerHealth;
        healthBar.value = playerHealth;
    }

    void UpdateHealthBar()
    {
        playerHealth = player.GetHealthValue();
        healthBar.maxValue = playerHealth;
        healthBar.value = playerHealth;
    }
    void UpdateXPBar()
    {
        if (!isMaxLevelReached)
            xpBar.maxValue = maxPlayerXP;
    }

    public void UpdateHealth(float damage)
    {
        playerHealth -= damage;
        healthBar.value = playerHealth;
        if (playerHealth <= 0)
        {
            EventService.Instance.OnGameOver();
        }
    }

    public void UpdateXP(float xp)
    {
        if (!isMaxLevelReached)
        {
            playerXP += xp;
            xpBar.value = playerXP;
            if (playerXP >= maxPlayerXP)
            {
                EventService.Instance.OnLevelEnd();
            }
        }
    }

    public int GetCurrentLevel()
    {
        return playerLevel;
    }

    public void ResetBars()
    {
        ResetXpBar();
        ResetHealthBar();
    }

    private void ResetXpBar()
    {
        if (!isMaxLevelReached)
        {
            xpBar.value = 0;
            playerXP = 0;
        }
    }

    private void ResetHealthBar()
    {
        healthBar.value = 100;
    }

    private void OnGameOver()
    {
        SetPlayer(false);
    }

    private void OnGameWin()
    {
        SetPlayer(false);
    }

    private void OnLevelEnd()
    {
        playerLevel++;
        player.SetPlayerLevel(playerLevel);
        if (playerLevel >= maxPlayerLevel)
        {
            EventService.Instance.OnMaxLevelReached();
            isMaxLevelReached = true;
        }
        SetPlayer(false);
        //Debug.Log("current level:" + playerLevel);
    }

    private void OnLevelUp()
    {
        if (!isMaxLevelReached)
        {
            maxPlayerXP *= playerLevel;
            xpBar.maxValue = maxPlayerXP;
        }
        SetPlayer(true);
        ResetBars();
        UIManager.Instance.SetLevelText(playerLevel);
    }

    private void OnPlayerHealthIncreased()
    {
        player.IncreaseHealth(healthIncreamentValue);
        UpdateHealthBar();
    }

    private void OnMaxLevelReached()
    {
        xpBar.gameObject.SetActive(false);
    }

    private void SetPlayer(bool flag)
    {
        player.GetComponent<PlayerMovement>().enabled = flag;
        player.gameObject.SetActive(flag);
    }
}
