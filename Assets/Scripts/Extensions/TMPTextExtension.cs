using TMPro;

internal static class TMPTextExtension
{
    public delegate string Formatter<in T>(T value);

    public static void Bind<T>(this TMP_Text text, Listenable<T> listenable, Formatter<T> formatter = null)
    {
        text.text = formatter != null ? formatter.Invoke(listenable.Value) : listenable.Value.ToString();
        listenable.OnValueChanged +=
            value => text.text = formatter != null ? formatter.Invoke(value) : value.ToString();
    }
}