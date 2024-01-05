using TMPro;
using UnityEngine;

public class WaveHUD : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private TMP_Text waveText;

    private void GameStarted()
    {
        timerText.Bind(GameManager.Current.timer, value => $"Prochaine vague dans {Mathf.Ceil(value)} secondes");
        waveText.Bind(GameManager.Current.currentWave, value => $"Vague {value}");
    }
}