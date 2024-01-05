using Mirror;

public partial class GameManager
{
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
}