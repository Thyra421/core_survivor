using UnityEngine;
using UnityEngine.UI;

public class AbilitiesHUD : MonoBehaviour
{
    [SerializeField] private Image ability1Image;
    [SerializeField] private Image ability2Image;
    [SerializeField] private Image ability3Image;

    public void Initialize(Player player)
    {
        player.Attack.Cooldown.CurrentValue.OnValueChanged +=
            _ => SetFilledAmount(ability1Image, player.Attack.Cooldown);

        player.Movement.DashCooldown.CurrentValue.OnValueChanged +=
            _ => SetFilledAmount(ability2Image, player.Movement.DashCooldown);

        player.Attack.Cooldown.CurrentValue.OnValueChanged +=
            _ => SetFilledAmount(ability3Image, player.Attack.Cooldown);
    }

    private void SetFilledAmount(Image image, Cooldown cooldown)
    {
        image.fillAmount = cooldown.CurrentValue.Value / cooldown.MaxValue;
    }
}