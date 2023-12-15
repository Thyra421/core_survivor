using System;
using UnityEngine.UI;

internal static class SliderExtension
{
    public static void BindValue(this Slider slider, Listenable<int> listenable, Action<int> callback = null)
    {
        slider.value = listenable.Value;
        callback?.Invoke(listenable.Value);
        
        listenable.OnValueChanged += value =>
        {
            slider.value = value;
            callback?.Invoke(value);
        };
    }

    public static void BindMaxValue(this Slider slider, Listenable<int> listenable)
    {
        slider.maxValue = listenable.Value;
        listenable.OnValueChanged += value => slider.maxValue = value;
    }
}