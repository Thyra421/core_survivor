using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private TMP_Text reasonText;

    [SerializeField]
    private Button leaveButton;

    [Server]
    private void Leave()
    {
        NetworkManager.singleton.ServerChangeScene("Draft");
    }

    public void Initialize(string reason)
    {
        reasonText.text = reason;
    }

    private void Awake()
    {
        // leaveButton.onClick.AddListener(Leave);
        // leaveButton.gameObject.SetActive(PlayerManager.Current.LocalPlayer.Value.isServer);
    }
}