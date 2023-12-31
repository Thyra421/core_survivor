﻿using UnityEngine;
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

        radioactivitySlider.maxValue = Radioactivity.MaxValue;
        radioactivitySlider.BindValue(player.Class.Radioactivity.Current);
    }
}