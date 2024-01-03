using System;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Object = UnityEngine.Object;

[Serializable]
public class MachineGunShoot : AbilityBase, ITargeted
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private int bulletDamages;

    [SerializeField]
    private Transform gunTip;

    [SerializeField]
    private Rig rig;

    private static readonly int IsShootingId = Animator.StringToHash("IsShooting");

    public Vector3? Target { get; private set; }
    public override bool IsChanneled => true;

    public string Serialize(Vector3 target)
    {
        return JsonUtility.ToJson(new MessageTarget(target));
    }

    public override void ClientUse(string args)
    {
        if (player.isOwned)
            Cooldown.Start();
        rig.weight = 1;
    }

    public override void ServerUse(string args)
    {
        MessageTarget message = JsonUtility.FromJson<MessageTarget>(args);
        Target = message.target;

        Cooldown.Start();
        IsCompleted = false;
        player.Animation.SetBool(IsShootingId, true);

        Vector3 shootDirection = (Target.Value - gunTip.position).normalized;
        shootDirection.y = 0;

        MachineGunBullet bullet = Object.Instantiate(bulletPrefab, gunTip.position,
            Quaternion.LookRotation(shootDirection)).GetComponent<MachineGunBullet>();
        bullet.Initialize(bulletDamages);
        NetworkServer.Spawn(bullet.gameObject);

        rig.weight = 1;
    }

    public override void ClientEnd(string args)
    {
        IsCompleted = true;
        Target = null;
        rig.weight = 0;
    }

    public override void ServerEnd(string args)
    {
        player.Animation.SetBool(IsShootingId, false);
        IsCompleted = true;
        Target = null;
        rig.weight = 0;
    }
}