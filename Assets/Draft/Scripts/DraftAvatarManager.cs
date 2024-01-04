using UnityEngine;

public class DraftAvatarManager : Singleton<DraftAvatarManager>
{
    [SerializeField]
    private Transform[] spots;

    [SerializeField]
    private GameObject cannoneerAvatarPrefab;

    [SerializeField]
    private GameObject demolisherAvatarPrefab;

    [SerializeField]
    private Material[] materials;

    protected override void Awake()
    {
        base.Awake();

        // Initialize with current data
        OnPlayersChanged(LobbyManager.Current.Players);
        LobbyManager.Current.Players.OnChanged += OnPlayersChanged;
    }

    private void OnDestroy()
    {
        LobbyManager.Current.Players.OnChanged -= OnPlayersChanged;
    }

    private void OnPlayersChanged(ListenableList<LobbyPlayerInfo> players)
    {
        for (int i = 0; i < 4; i++) {
            if (spots[i].childCount > 0)
                Destroy(spots[i].GetChild(0).gameObject);

            if (players.Count <= i) continue;

            DraftAvatar avatar =
                Instantiate(players[i].Class == Class.cannoneer ? cannoneerAvatarPrefab : demolisherAvatarPrefab,
                    spots[i]).GetComponent<DraftAvatar>();

            avatar.Initialize(players[i].Name, materials[i]);
        }
    }
}