public class EnvironmentManager : Singleton<EnvironmentManager>
{
    public ListenableList<DestructibleEnvironment> Environments { get; } = new();
}
