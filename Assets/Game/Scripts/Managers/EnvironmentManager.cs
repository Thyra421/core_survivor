using UnityEngine;

public class EnvironmentManager : Singleton<EnvironmentManager>
{
    public ListenableList<DestructibleEnvironment> Environments { get; } = new();

    [SerializeField]
    private Transform core;

    public Transform Core => core;
}