using System;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Flamethrower : AbilityBase, ITargeted
{
    [SerializeField]
    private GameObject flames;

    [SerializeField]
    private int tickDamage;

    [SerializeField]
    private Transform gunTip;

    [SerializeField]
    private float radius;

    [SerializeField]
    private float distance;

    private static readonly int IsShootingId = Animator.StringToHash("IsShooting");

    public Vector3? Target { get; private set; }
    public override bool IsChanneled => true;

    public string Serialize(Vector3 target)
    {
        return JsonUtility.ToJson(new MessageTarget(target));
    }

    public override void ClientUse(string args)
    {
        flames.SetActive(true);
        if (player.isOwned)
            Cooldown.Start();
    }

    public override void ServerUse(string args)
    {
        MessageTarget message = JsonUtility.FromJson<MessageTarget>(args);
        Target = message.target;

        Cooldown.Start();
        IsCompleted = false;
        player.Animation.SetBool(IsShootingId, true);

        Vector3 direction = Target.Value - gunTip.position;

        Ray ray = new(gunTip.position, direction);

        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, distance);

        if (hits.Length <= 0) return;

        foreach (RaycastHit h in hits) {
            if (!h.transform.TryGetComponent(out EnemyHealth enemy)) continue;

            enemy.TakeDamage(tickDamage);
        }
    }

    public void ServerEnd()
    {
        player.Animation.SetBool(IsShootingId, false);
        IsCompleted = true;
        Target = null;
    }

    public void ClientEnd()
    {
        flames.SetActive(false);
        player.Animation.SetBool(IsShootingId, false);
        IsCompleted = true;
        Target = null;
    }
}