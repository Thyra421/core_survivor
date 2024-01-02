using UnityEngine;
using UnityEngine.UI;

public class DisconnectedFromServer : MonoBehaviour
{
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        menuButton.onClick.AddListener(OnClickMenu);
    }

    private void OnClickMenu()
    {
        SceneLoader.Current.LoadMenuAsync();
    }
}