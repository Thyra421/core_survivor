using Mirror;
using UnityEngine;

[RequireComponent(typeof(Player))]
public partial class PlayerAttack : NetworkBehaviour
{
    [SerializeField]
    private float attackDelay = .25f;

    [SerializeField]
    private float cooldownDuration = 1f;

    private Player _player;
    public Cooldown Cooldown { get; private set; }

    private void Awake()
    {
        Cooldown = new Cooldown(cooldownDuration);
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        Cooldown.Update();
    }
}