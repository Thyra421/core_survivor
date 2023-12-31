using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbiesHUD : MonoBehaviour
{
    [SerializeField] private RectTransform lobbiesRoot;
    [SerializeField] private GameObject lobbiesEntryPrefab;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private TMP_InputField createLobbyNameInputField;

    private void OnLobbiesChanged(ListenableList<LobbyInformations> lobbies)
    {
        for (int i = lobbiesRoot.childCount - 1; i >= 0; i--)
            Destroy(lobbiesRoot.GetChild(i).gameObject);

        foreach (LobbyInformations lobby in lobbies) {
            GameObject newEntry = Instantiate(lobbiesEntryPrefab, lobbiesRoot);
            LobbiesEntryHUD hud = newEntry.GetComponent<LobbiesEntryHUD>();

            hud.Initialize(lobby);
        }
    }

    private void OnClickCreateRoom()
    {
        LobbyManager.Current.CreateAndJoinLobby();
    }

    private void Start()
    {
        LobbiesManager.Current.Lobbies.OnChanged += OnLobbiesChanged;
        OnLobbiesChanged(LobbiesManager.Current.Lobbies);
        createLobbyButton.onClick.AddListener(OnClickCreateRoom);
    }

    private void OnDestroy()
    {
        LobbiesManager.Current.Lobbies.OnChanged -= OnLobbiesChanged;
    }
}