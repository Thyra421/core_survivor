using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public class Flamethrower : ProgressiveAbility
{
    [SerializeField]
    private ParticleSystem flames;

    [SerializeField]
    private Transform gunTip;

    [SerializeField]
    private float radius;

    [SerializeField]
    private float distance;

    [SerializeField]
    private Rig rig;
    
    [SerializeField]
    private AudioSource audio;

    private static readonly int IsShootingId = Animator.StringToHash("IsShooting");

    public string Serialize(Vector3 target)
    {
        return JsonUtility.ToJson(new MessageTarget(target));
    }

    public override void ClientStartUsing()
    {
        base.ClientStartUsing();
        flames.Play();
        rig.weight = 1;
        audio.Play();
    }

    public override void ServerStartUsing()
    {
        base.ServerStartUsing();
        player.Animation.SetBool(IsShootingId, true);
        rig.weight = 1;
    }

    protected override void ServerUsing()
    {
        base.ServerUsing();
        Vector3 direction = player.Class.Target - gunTip.position;
        Ray ray = new(gunTip.position, direction);
        RaycastHit[] hits = Physics.SphereCastAll(ray, radius, distance);

        if (hits.Length > 0)
            foreach (RaycastHit h in hits) {
                if (!h.transform.TryGetComponent(out EnemyHealth enemy)) continue;

                // check that enemy is in line of sight
                Vector3 enemyPosition = h.transform.position;
                Ray enemyRay = new(gunTip.position, h.transform.position - gunTip.position);
                if (!Physics.Raycast(enemyRay, Vector3.Distance(enemyPosition, gunTip.position),
                        LayerManager.Current.WhatIsObstacle))
                    enemy.TakeDamage(damages);
            }

        if (player.Class.Radioactivity.Current.Value < cost)
            player.Class.ForceStopUsingAbility(this);
    }

    public override void ClientStopUsing()
    {
        base.ClientStopUsing();
        flames.Stop();
        rig.weight = 0;
        audio.Stop();
    }

    public override void ServerStopUsing()
    {
        base.ServerStopUsing();
        player.Animation.SetBool(IsShootingId, false);
        rig.weight = 0;
    }
}