using UnityEngine.UI;

internal static class SliderExtension
{
    public static void BindValue(this Slider slider, Listenable<int> listenable)
    {
        slider.value = listenable.Value;
        listenable.OnValueChanged += value => slider.value = value;
    }

    public static void BindMaxValue(this Slider slider, Listenable<int> listenable)
    {
        slider.maxValue = listenable.Value;
        listenable.OnValueChanged += value => slider.maxValue = value;
    }
}