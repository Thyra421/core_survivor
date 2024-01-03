using System;
using Mirror;
using UnityEngine;
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
    }

    public override void ServerUse(string args)
    {
        MessageTarget message = JsonUtility.FromJson<MessageTarget>(args);
        Target = message.target;

        Cooldown.Start();
        IsCompleted = false;

        MachineGunBullet bullet = Object.Instantiate(bulletPrefab, gunTip.position,
            Quaternion.LookRotation(Target.Value - player.transform.position)).GetComponent<MachineGunBullet>();
        bullet.Initialize(bulletDamages);
        NetworkServer.Spawn(bullet.gameObject);
    }

    public void End()
    {
        IsCompleted = true;
        Target = null;
    }
}