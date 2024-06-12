using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class EventService : MonoSingletonGeneric<EventService>
{
    public event Action PlayerSpawn;
    public event Action GameStart;
    public event Action GameOver;
    public event Action GameWin;
    public event Action ProjectileDamageIncrement;
    public event Action ProjectileSpawnSpeedIncrement;
    public event Action PlayerHealthIncrement;
    public event Action LevelUP;
    public event Action LevelEnd;
    public event Action MaxLevelReached;
  

    public void OnPlayerSpawn()
    {
        PlayerSpawn?.Invoke();
    }
    
    public void OnGameStart()
    {
        GameStart?.Invoke();
    }

    public void OnGameOver()
    {
        Debug.Log("game Over");
        GameOver?.Invoke();
    }
    public void OnGameWin()
    {
        GameWin?.Invoke();
    }
    public void OnProjectileDamageIncrement()
    {
        ProjectileDamageIncrement?.Invoke();
    }

    public void OnProjectileSpeedIncreased()
    {
        ProjectileSpawnSpeedIncrement?.Invoke();
    }
    public void OnPlayerHealthIncreased()
    {
        PlayerHealthIncrement?.Invoke();
    }

    public void OnLevelUP()
    {
        LevelUP?.Invoke();
    }  
    public void OnLevelEnd()
    {
        LevelEnd?.Invoke();
    }
    public void OnMaxLevelReached()
    {
        MaxLevelReached?.Invoke();
    }

   

}