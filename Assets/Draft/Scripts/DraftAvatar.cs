using TMPro;
using UnityEngine;

public class DraftAvatar : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;

    public void Initialize(string username)
    {
        nameText.text = username;
    }
}