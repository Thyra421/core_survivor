using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSelectionEntryHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text gameNameText;
    [SerializeField] private Button joinGameButton;

    public void Initialize(GameInstance instance)
    {
        joinGameButton.onClick.AddListener(() => { InstanceModeManager.ServerPort = instance.port; });
        gameNameText.text = instance.name;
    }
}