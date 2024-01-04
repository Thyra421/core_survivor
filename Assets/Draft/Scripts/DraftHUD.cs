using UnityEngine;
using UnityEngine.UI;

public class DraftHUD : MonoBehaviour
{
    [SerializeField] private Button leaveButton;

    [SerializeField] private Button startButton;
    
    [SerializeField] private Button demolisherButton;

    [SerializeField] private Button cannoneerButton;

    private void Awake()
    {
        leaveButton.onClick.AddListener(DraftManager.Current.LeaveDraft);
        startButton.onClick.AddListener(DraftManager.Current.StartGame);
        cannoneerButton.onClick.AddListener(DraftManager.Current.PickCannoneer);
        demolisherButton.onClick.AddListener(DraftManager.Current.PickDemolisher);
        startButton.gameObject.SetActive(LobbyManager.Current.IsHost);
    }

    private void OnDestroy()
    {
        leaveButton.onClick.RemoveListener(DraftManager.Current.LeaveDraft);
        startButton.onClick.RemoveListener(DraftManager.Current.StartGame);
        cannoneerButton.onClick.RemoveListener(DraftManager.Current.PickCannoneer);
        demolisherButton.onClick.RemoveListener(DraftManager.Current.PickDemolisher);
    }
}