using UnityEngine;
using UnityEngine.UI;

public class DraftHUD : MonoBehaviour
{
    [SerializeField] private Button leaveButton;

    [SerializeField] private Button startButton;

    private void Awake()
    {
        leaveButton.onClick.AddListener(DraftManager.Current.LeaveDraft);
        startButton.onClick.AddListener(DraftManager.Current.StartGame);
        startButton.gameObject.SetActive(LobbyManager.Current.IsHost);
    }

    private void OnDestroy()
    {
        leaveButton.onClick.RemoveListener(DraftManager.Current.LeaveDraft);
        startButton.onClick.RemoveListener(DraftManager.Current.StartGame);
    }
}