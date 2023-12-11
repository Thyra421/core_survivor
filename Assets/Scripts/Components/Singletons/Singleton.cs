using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Current { get; private set; }

    protected virtual void Awake() {
        if (Current == null)
            Current = this as T;
        else
            Destroy(gameObject);
    }
}
