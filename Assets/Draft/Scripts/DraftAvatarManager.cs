using System;
using UnityEngine;

public class DraftAvatarManager : Singleton<DraftAvatarManager>
{
    [SerializeField] private Transform[] spots;
    [SerializeField] private GameObject draftAvatarPrefab;

    protected override void Awake()
    {
        base.Awake();

        if (LobbyManager.Current == null) throw new Exception("Not in a lobby");

        // Initialize with current data
        OnPlayersChanged(LobbyManager.Current.Players);
        LobbyManager.Current.Players.OnChanged += OnPlayersChanged;
    }

    private void OnDestroy()
    {
        if (LobbyManager.Current == null) throw new Exception("Not in a lobby");

        LobbyManager.Current.Players.OnChanged -= OnPlayersChanged;
    }

    private void OnPlayersChanged(ListenableList<LobbyPlayerInfo> players)
    {
        for (int i = 0; i < 4; i++) {
            if (spots[i].childCount > 0)
                Destroy(spots[i].GetChild(0).gameObject);

            if (players.Count <= i) continue;

            DraftAvatar avatar = Instantiate(draftAvatarPrefab, spots[i]).GetComponent<DraftAvatar>();

            avatar.Initialize(players[i].Name);
        }
    }
}