using System;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class MachineGunShoot : AbilityBase, ITargeted
{
    [SerializeField]
    private GameObject bulletPrefab;

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

        GameObject bullet = Object.Instantiate(bulletPrefab, player.transform.position, player.transform.rotation);
        NetworkServer.Spawn(bullet);
    }

    public void End()
    {
        IsCompleted = true;
        Target = null;
    }
}