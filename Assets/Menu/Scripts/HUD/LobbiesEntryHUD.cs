using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesEntryHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private Button joinGameButton;

    private void OnClickJoin(LobbyInformation lobbyInformation)
    {
        LobbyManager.Current.JoinLobby(lobbyInformation.id);
    }

    public void Initialize(LobbyInformation lobbyInformation)
    {
        joinGameButton.onClick.AddListener(() => OnClickJoin(lobbyInformation));
        gameNameText.text = lobbyInformation.name;
    }
}