using Steamworks;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    private SteamworksAPI _steamworksAPI;

    public bool IsHost { get; private set; }
    public string NetworkAddress { get; private set; }

    private void OnCreatedLobby(ulong id)
    {
        MasterServer.Current.CreateLobby(id);
    }

    private void OnJoinedLobby(ulong id, bool isHost)
    {
        SteamLobbyInformation steamLobbyInformation = SteamworksHelper.GetCurrentLobbyInformation(id);

        NetworkAddress = steamLobbyInformation.networkAddress;
        IsHost = isHost;

        SceneLoader.Current.LoadGameAsync();
    }

    public void CreateAndJoinLobby()
    {
        _steamworksAPI.HostLobby(4);
    }

    public void JoinLobby(ulong lobbyId)
    {
        _steamworksAPI.JoinLobby(lobbyId);
    }

    protected override void Awake()
    {
        base.Awake();
        _steamworksAPI = new SteamworksAPI(OnCreatedLobby, OnJoinedLobby);
    }
}