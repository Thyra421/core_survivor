using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T _current;

    public static T Current {
        get {
            if (_current == null) {
                T[] assets = Resources.LoadAll<T>("");

                if (assets == null || assets.Length < 1)
                    throw new System.Exception($"Could not find an instance of {typeof(T).Name}.");
                else if (assets.Length > 1)
                    throw new System.Exception($"Multiple instances of {typeof(T).Name} found.");
                _current = assets[0];
            }
            return _current;
        }
    }
}