using Mirror;
using UnityEngine;

public partial class GameManager
{
    [SerializeField]
    private GameObject gameOverPrefab;
    public readonly Listenable<int> currentWave = new(1);

    [ClientRpc]
    private void SetTimerRpc(float duration)
    {
        timer.Value = duration;
    }

    [ClientRpc]
    private void SetWaveRpc(int wave)
    {
        currentWave.Value = wave;
    }

    [ClientRpc]
    private void GameOverRpc(string reason)
    {
        GameOver gameOver = Instantiate(gameOverPrefab).GetComponent<GameOver>();
        gameOver.Initialize(reason);
    }
}