using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingletonGeneric<UIManager>
{
    [SerializeField]
    private GameObject gameOverUIPanel;
    [SerializeField]
    private Text gameOverUIMainText; 
    [SerializeField]
    private GameObject levelUpUiPanel;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private int timeToWin;
    [SerializeField]
    private Text levelText;

    private string timerTextString;
    private float timer;
    private bool isRunning;

    protected override void Start()
    {
        EventService.Instance.GameOver += OnGameOver;
        EventService.Instance.GameWin += OnGameWin;
        EventService.Instance.LevelUP += OnLevelUP;
        EventService.Instance.LevelEnd += OnLevelEnd;
        isRunning = true;
        levelText.text = "Level: " + PlayerManager.Instance.GetCurrentLevel();
    }

    protected override void Update()
    {
        // Running the timer for winning time of game for playwer

        if (isRunning)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerTextString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = timerTextString;
            //Debug.Log("seconds: " + timerTextString);

            if (minutes == timeToWin)
            {
                isRunning = false;
                EventService.Instance.OnGameWin();
            }
        }
    }

    private void OnGameOver()
    {
        Debug.Log("game over ui");
        gameOverUIPanel.SetActive(true);
        gameOverUIMainText.text = "Game Over";
    }
    private void OnGameWin()
    {
        gameOverUIPanel.SetActive(true);
        gameOverUIMainText.text = "Game Win";
    }

    private void OnLevelUP()
    {
        isRunning = true;
        levelUpUiPanel.SetActive(false);

    }

    private void OnLevelEnd()
    {
        isRunning = false;
        levelUpUiPanel.SetActive(true);
    }

    public void SetLevelText(int _level)
    {
        levelText.text = "Level: " + _level;
    }

    public void OnIcreaseAttackSpedSelect()
    {
        EventService.Instance.OnProjectileSpeedIncreased();
        EventService.Instance.OnLevelUP();

    }

    public void OnIncreaseDamageSelect()
    {
        EventService.Instance.OnProjectileDamageIncrement();
        EventService.Instance.OnLevelUP();
    }

    public void OnIncreaseHealthSelect()
    {
        EventService.Instance.OnPlayerHealthIncreased();
        EventService.Instance.OnLevelUP();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
