using UnityEngine;
using UnityEngine.UI;

public class AbilitiesHUD : MonoBehaviour
{
    [SerializeField]
    private Image ability1Image;

    [SerializeField]
    private Image ability2Image;

    [SerializeField]
    private Image ability3Image;

    public void Initialize(Player player)
    {
        BindAbility(player.Class.Abilities[0], ability1Image);
    }

    private void BindAbility(AbilityBase ability, Image image)
    {
        ability.Cooldown.CurrentValue.OnValueChanged +=
            _ => SetFilledAmount(image, ability.Cooldown);
    }

    private void SetFilledAmount(Image image, Cooldown cooldown)
    {
        image.fillAmount = cooldown.ProgressRatio;
    }
}