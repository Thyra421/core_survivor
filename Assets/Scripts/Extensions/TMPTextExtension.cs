using TMPro;

internal static class TMPTextExtension
{
    public static void Bind(this TMP_Text text, Listenable<string> listenable)
    {
        text.text = listenable.Value;
        listenable.OnValueChanged += value => text.text = value;
    }
}