using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesEntryHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private Button joinGameButton;

    private void OnClickJoin(LobbyInformations lobbyInformations)
    {
        LobbyManager.Current.JoinLobby(ulong.Parse(lobbyInformations.name));
    }

    public void Initialize(LobbyInformations lobbyInformations)
    {
        joinGameButton.onClick.AddListener(() => OnClickJoin(lobbyInformations));
        gameNameText.text = lobbyInformations.name;
    }
}