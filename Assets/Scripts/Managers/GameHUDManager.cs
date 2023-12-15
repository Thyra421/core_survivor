using UnityEngine;

public class GameHUDManager : Singleton<GameHUDManager>
{
    [SerializeField] private ResourcesHUD resourcesHUD;

    private void Start()
    {
        PlayerManager.Current.LocalPlayer.OnValueChanged += OnLocalPlayerChanged;
    }

    private void OnLocalPlayerChanged(Player player)
    {
        if (player != null)
            resourcesHUD.Initialize(player);
    }
}