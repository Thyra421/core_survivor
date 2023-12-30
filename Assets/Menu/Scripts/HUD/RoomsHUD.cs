using System.Linq;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomsHUD : MonoBehaviour
{
    [SerializeField] private RectTransform gamesContentRoot;
    [SerializeField] private GameObject gameSelectionEntryPrefab;
    [SerializeField] private Button createGameButton;
    [SerializeField] private TMP_InputField createGameNameInputField;

    private void OnGameInstancesChanged(ListenableList<Room> instances)
    {
        for (int i = gamesContentRoot.childCount - 1; i >= 0; i--)
            Destroy(gamesContentRoot.GetChild(i).gameObject);

        foreach (Room i in instances) {
            GameObject newEntry = Instantiate(gameSelectionEntryPrefab, gamesContentRoot);
            RoomsEntryHUD hud = newEntry.GetComponent<RoomsEntryHUD>();

            hud.Initialize(i);
        }
    }

    private void OnClickCreateRoom()
    {
        IPAddress localAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList
            .First(a => a.AddressFamily == AddressFamily.InterNetwork);

        MasterServer.Current.Send(
            new CreateRoomMessage
            {
                action = "create",
                port = 7777,
                ip = localAddress.ToString(),
                name = createGameNameInputField.text
            });
        MasterServer.Current.GameInfo = new GameInfo(InstanceMode.Client, 7777, "localhost");
    }

    private void Start()
    {
        RoomsManager.Current.Rooms.OnChanged += OnGameInstancesChanged;
        OnGameInstancesChanged(RoomsManager.Current.Rooms);
        createGameButton.onClick.AddListener(OnClickCreateRoom);
    }
}