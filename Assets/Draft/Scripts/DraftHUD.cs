using UnityEngine;
using UnityEngine.UI;

public class DraftHUD : MonoBehaviour
{
    [SerializeField] private Button leaveButton;

    private void Awake()
    {
        leaveButton.onClick.AddListener(DraftManager.Current.LeaveDraft);
    }
}
