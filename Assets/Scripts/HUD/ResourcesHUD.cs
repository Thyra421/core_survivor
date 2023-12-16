using UnityEngine;
using UnityEngine.UI;

public class ResourcesHUD : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;

    public void Initialize(Player player)
    {
        healthSlider.BindMaxValue(player.Health.max);
        healthSlider.BindValue(player.Health.current);
        staminaSlider.BindValue(player.Movement.Stamina);
        staminaSlider.maxValue = 100;
    }
}