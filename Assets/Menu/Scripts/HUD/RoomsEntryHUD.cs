using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomsEntryHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private Button joinGameButton;

    public void Initialize(Room room)
    {
        joinGameButton.onClick.AddListener(() =>
        {
            GameInfo gameInfo = new(InstanceMode.Client, room.port, room.networkAddress);

            MasterServer.Current.GameInfo = gameInfo;
        });
        gameNameText.text = room.name;
    }
}