using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class GameInstanceManager : NetworkSingleton<GameInstanceManager>
{
    private readonly SyncList<GameInstance> _gameInstancesSync = new();
    public ListenableList<GameInstance> GameInstances { get; } = new();

    private void GameInstanceCallback(SyncList<GameInstance>.Operation op, int index, GameInstance oldItem,
        GameInstance newItem)
    {
        switch (op)
        {
            case SyncList<GameInstance>.Operation.OP_ADD:
                GameInstances.Add(newItem);
                break;
            case SyncList<GameInstance>.Operation.OP_INSERT:
                break;
            case SyncList<GameInstance>.Operation.OP_REMOVEAT:
                GameInstances.RemoveAt(index);
                break;
            case SyncList<GameInstance>.Operation.OP_SET:
                break;
            case SyncList<GameInstance>.Operation.OP_CLEAR:
                break;
        }
    }

    public void Add(GameInstance instance)
    {
        _gameInstancesSync.Add(instance);
    }

    public void Remove(GameInstance instance)
    {
        _gameInstancesSync.Remove(instance);
    }

    protected void Start()
    {
        _gameInstancesSync.Callback += GameInstanceCallback;
        for (int index = 0; index < _gameInstancesSync.Count; index++)
            GameInstanceCallback(SyncList<GameInstance>.Operation.OP_ADD, index, new GameInstance(),
                _gameInstancesSync[index]);
    }
}