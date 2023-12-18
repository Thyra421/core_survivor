using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

public class InstanceModeHUD : MonoBehaviour
{
    [SerializeField] private Button masterServerButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button dedicatedInstanceButton;
    [SerializeField] private TMP_InputField dedicatedInstancePortInputField;

    private static void OnClickMasterServer()
    {
        InstanceModeManager.Mode = InstanceMode.MasterServer;
        SceneManager.LoadSceneAsync("Menu");
    }

    private static void OnClickClient()
    {
        InstanceModeManager.Mode = InstanceMode.Client;
        SceneManager.LoadSceneAsync("Menu");
    }

    private void OnClickDedicatedInstance()
    {
        if (!int.TryParse(dedicatedInstancePortInputField.text, out int port)) return;

        InstanceModeManager.Mode = InstanceMode.DedicatedServer;
        InstanceModeManager.ServerPort = port;
        SceneManager.LoadSceneAsync("Lobby");
    }

    private void Start()
    {
        masterServerButton.onClick.AddListener(OnClickMasterServer);
        clientButton.onClick.AddListener(OnClickClient);
        dedicatedInstanceButton.onClick.AddListener(OnClickDedicatedInstance);
    }
}