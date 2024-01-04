using UnityEngine;
using UnityEngine.UI;

namespace CoreSurvivor
{
    public class LobbiesHUD : MonoBehaviour
    {
        [SerializeField] private RectTransform lobbiesRoot;
        [SerializeField] private GameObject lobbiesEntryPrefab;
        [SerializeField] private Button createLobbyButton;

        private void OnLobbiesChanged(ListenableList<LobbyInformation> lobbies)
        {
            for (int i = lobbiesRoot.childCount - 1; i >= 0; i--)
                Destroy(lobbiesRoot.GetChild(i).gameObject);

            foreach (LobbyInformation lobby in lobbies) {
                GameObject newEntry = Instantiate(lobbiesEntryPrefab, lobbiesRoot);
                LobbiesEntryHUD hud = newEntry.GetComponent<LobbiesEntryHUD>();

                hud.Initialize(lobby);
            }
        }

        private void OnClickCreateRoom()
        {
            LobbyManager.Current.HostLobby();
        }

        private void Start()
        {
            OnLobbiesChanged(LobbiesManager.Current.Lobbies);
            LobbiesManager.Current.Lobbies.OnChanged += OnLobbiesChanged;
        }

        private void Awake()
        {
            createLobbyButton.onClick.AddListener(OnClickCreateRoom);
        }

        private void OnDestroy()
        {
            LobbiesManager.Current.Lobbies.OnChanged -= OnLobbiesChanged;
        }
    }
}