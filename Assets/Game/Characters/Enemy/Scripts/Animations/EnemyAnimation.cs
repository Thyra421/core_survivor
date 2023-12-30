using Mirror;
using UnityEngine;

public partial class EnemyAnimation : NetworkBehaviour
{
    private Animator _animator;

    private void Update()
    {
        if (isServer)
            ServerUpdate();
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }
}