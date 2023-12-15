using UnityEngine;
using UnityEngine.UI;

public class ResourcesHUD : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    public void Initialize(Player player)
    {
        healthSlider.BindMaxValue(player.Health.max);
        healthSlider.BindValue(player.Health.current);
    }
}