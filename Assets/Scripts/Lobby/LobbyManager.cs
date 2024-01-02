using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private GameObject disconnectedFromLobbyPrefab;
    private SteamworksLobbyAPI _steamworksLobbyAPI;

    public ListenableList<LobbyPlayerInfo> Players { get; } = new();
    public bool IsHost { get; private set; }
    public string NetworkAddress { get; private set; }

    private ulong _lobbyId;
    private ulong _lobbyOwnerId;

    private void OnCreatedLobby(ulong lobbyId)
    {
        MasterServer.Current.CreateLobby(lobbyId);

        ConsoleLogger.Steamworks($"Created lobby");
    }

    private void OnJoinedLobby(ulong lobbyId, bool isHost)
    {
        SteamLobbyInformation steamLobbyInformation = SteamworksHelper.GetCurrentLobbyInformation(lobbyId);
        List<LobbyPlayerInfo> playersInLobby = SteamworksHelper.GetPlayersInLobby(lobbyId);
        CSteamID ownerId = SteamMatchmaking.GetLobbyOwner(new CSteamID(lobbyId));

        Players.AddRange(playersInLobby);
        _lobbyOwnerId = ownerId.m_SteamID;
        _lobbyId = lobbyId;
        NetworkAddress = steamLobbyInformation.networkAddress;
        IsHost = isHost;

        ConsoleLogger.Steamworks($"Joined lobby {steamLobbyInformation.name}");

        SceneLoader.Current.LoadDraftAsync();
    }

    private void OnUserJoinedLobby(ulong userId)
    {
        LobbyPlayerInfo lobbyPlayerInfo = SteamworksHelper.GetPlayerInfo(userId);

        Players.Add(lobbyPlayerInfo);

        ConsoleLogger.Steamworks($"Player {lobbyPlayerInfo.Name} joined lobby");
    }

    private void OnUserLeftLobby(ulong userId)
    {
        int index = Players.FindIndex(p => p.Id == userId);

        if (index == -1) return;

        Players.RemoveAt(index);

        if (userId == _lobbyOwnerId) {
            LeaveLobby();
            Instantiate(disconnectedFromLobbyPrefab);
        }

        ConsoleLogger.Steamworks($"Player left lobby");
    }

    public void HostLobby()
    {
        _steamworksLobbyAPI.HostLobby(4);
    }

    public void JoinLobby(ulong lobbyId)
    {
        _steamworksLobbyAPI.JoinLobby(lobbyId);
    }


    public void LeaveLobby()
    {
        _steamworksLobbyAPI.LeaveLobby(_lobbyId);
        Players.Clear();
        if (IsHost)
            MasterServer.Current.DeleteLobby();
    }

    // ReSharper disable once UnusedMember.Local
    private void SteamStarted()
    {
        _steamworksLobbyAPI = new SteamworksLobbyAPI(OnCreatedLobby, OnJoinedLobby, OnUserJoinedLobby, OnUserLeftLobby);
    }
}