using UnityEngine;
using UnityEngine.UI;

public class ResourcesHUD : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider energySlider;
    [SerializeField] private Slider staminaSlider;

    public void Initialize(Player player)
    {
        healthSlider.maxValue = player.Health.Max;
        healthSlider.BindValue(player.Health.Current);
        
        staminaSlider.maxValue = 100;
        staminaSlider.BindValue(player.Movement.stamina);

        energySlider.maxValue = 100;
        energySlider.BindValue(player.Attack.Energy);
    }
}