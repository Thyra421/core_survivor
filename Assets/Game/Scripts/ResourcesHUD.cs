using UnityEngine;
using UnityEngine.UI;

public class ResourcesHUD : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private Slider radioactivitySlider;

    [SerializeField]
    private Slider staminaSlider;

    public void Initialize(Player player)
    {
        healthSlider.maxValue = player.Health.Max;
        healthSlider.BindValue(player.Health.Current);

        staminaSlider.maxValue = 100;
        staminaSlider.BindValue(player.Movement.stamina);

        if (player.Class is IRadioactivityUser radioactivityUser) {
            radioactivitySlider.maxValue = 100;
            radioactivitySlider.BindValue(radioactivityUser.Radioactivity.Value);
        }
    }
}