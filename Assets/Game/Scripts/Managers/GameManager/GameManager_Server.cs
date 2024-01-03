using System;
using Mirror;
using UnityEngine;

public partial class GameManager
{
    private float _timer;

    [Server]
    private void ServerUpdate()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0) {
            StartCoroutine(EnemyManager.Current.SpawnWave(8));
            
            _timer = 12;
        }

        int currentSecond = (int)Math.Floor(_timer);
        _timerSync = currentSecond;
    }
}