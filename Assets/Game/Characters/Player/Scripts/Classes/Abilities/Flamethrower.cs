using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public class Flamethrower : AbilityBase, ITargeted
{
    [SerializeField]
    private ParticleSystem flames;

    [SerializeField]
    private int tickDamage;

    [SerializeField]
    private Transform gunTip;

    [SerializeField]
    private float radius;

    [SerializeField]
    private float distance;

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
        flames.Play();
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

        Vector3 direction = Target.Value - gunTip.position;

        Ray ray = new(gunTip.position, direction);

        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, distance);

        if (hits.Length <= 0) return;

        foreach (RaycastHit h in hits) {
            if (!h.transform.TryGetComponent(out EnemyHealth enemy)) continue;

            // check that enemy is in line of sight
            Vector3 enemyPosition = h.transform.position;
            Ray enemyRay = new(gunTip.position, h.transform.position - gunTip.position);
            if (!Physics.Raycast(enemyRay, Vector3.Distance(enemyPosition, gunTip.position),
                    LayerManager.Current.WhatIsObstacle))
                enemy.TakeDamage(tickDamage);
        }

        rig.weight = 1;
    }

    public override void ClientEnd(string args)
    {
        flames.Stop();
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