using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesEntryHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private Button joinGameButton;

    private void OnClickJoin(Lobby lobby)
    {
        LobbyManager.Current.JoinLobby(ulong.Parse(lobby.name));
    }

    public void Initialize(Lobby lobby)
    {
        joinGameButton.onClick.AddListener(() => OnClickJoin(lobby));
        gameNameText.text = lobby.name;
    }
}