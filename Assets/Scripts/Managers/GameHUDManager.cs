using TMPro;
using UnityEngine;

public class GameHUDManager : Singleton<GameHUDManager>
{
    [SerializeField] private ResourcesHUD resourcesHUD;
    [SerializeField] private TMP_Text timerText;

    private void Start()
    {
        PlayerManager.Current.LocalPlayer.OnValueChanged += OnLocalPlayerChanged;
        timerText.Bind(GameManager.Current.timer, value => $"Next wave in {Mathf.Floor(value)}s");
    }

    private void OnLocalPlayerChanged(Player player)
    {
        if (player != null)
            resourcesHUD.Initialize(player);
    }
}