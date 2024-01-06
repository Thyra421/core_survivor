using TMPro;
using UnityEngine;

public class GameHUDManager : Singleton<GameHUDManager>
{
    [SerializeField]
    private ResourcesHUD resourcesHUD;

    [SerializeField]
    private AbilitiesHUD abilitiesHUD;

    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private GameObject temporaryLoading;

    // ReSharper disable once UnusedMember.Local
    private void GameStarted()
    {
        PlayerManager.Current.LocalPlayer.OnValueChanged += OnLocalPlayerChanged;
        timerText.Bind(GameManager.Current.timer, value => $"Next wave in {Mathf.Floor(value)}s");
    }

    public void DestroyTemporaryLoading()
    {
        Destroy(temporaryLoading);
    }

    private void OnLocalPlayerChanged(Player player)
    {
        if (player != null) {
            resourcesHUD.Initialize(player);
            abilitiesHUD.Initialize(player);
        }
    }
}