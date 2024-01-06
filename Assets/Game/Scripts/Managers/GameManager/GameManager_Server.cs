using Mirror;

public partial class GameManager
{
    private static readonly WavePattern[] WavePatterns = {
        new() { walkers = 3, timer = 10 },
        new() { walkers = 2, hunters = 1, timer = 15 },
        new() { walkers = 5, timer = 10 },
        new() { walkers = 3, hunters = 2, timer = 15 },
        new() { walkers = 3, mutants = 1, timer = 20 },
    };

    private int _patternIndex;
    private int _waveMultiplicator = 1;

    [Server]
    public void GameOver(string reason)
    {
        GameOverRpc(reason);
    }

    [Server]
    private void ServerUpdate()
    {
        if (timer.Value > 0) return;

        WavePattern pattern = WavePatterns[_patternIndex];

        EnemyManager.Current.SpawnWave(pattern, _waveMultiplicator);

        timer.Value = pattern.timer;
        SetTimerRpc(pattern.timer);
        SetWaveRpc(_patternIndex + 1 + WavePatterns.Length * (_waveMultiplicator - 1));

        _patternIndex = (_patternIndex + 1) % WavePatterns.Length;
        if (_patternIndex == 0)
            _waveMultiplicator++;
    }
}

public struct WavePattern
{
    public int walkers;
    public int hunters;
    public int mutants;
    public int timer;
}