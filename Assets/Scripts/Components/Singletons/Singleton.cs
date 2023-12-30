using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] private bool dontDestroyOnLoad;
    public static T Current { get; private set; }

    protected virtual void Awake()
    {
        if (Current == null) {
            Current = this as T;
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}