using TMPro;
using UnityEngine;

public class DraftAvatar : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Renderer renderer;

    public void Initialize(string username, Material material)
    {
        nameText.text = username;
        renderer.material = material;
    }
}