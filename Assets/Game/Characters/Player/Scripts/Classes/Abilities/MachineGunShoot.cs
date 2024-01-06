using System;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Object = UnityEngine.Object;

[Serializable]
public class MachineGunShoot : ProgressiveAbility
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform gunTip;

    [SerializeField]
    private Rig rig;

    [SerializeField]
    private int radioactivityPerHit;

    [SerializeField]
    private AudioSource audio;

    private static readonly int IsShootingId = Animator.StringToHash("IsShooting");

    public override void ClientStartUsing()
    {
        base.ClientStartUsing();
        rig.weight = 1;
    }

    public override void ServerStartUsing()
    {
        base.ServerStartUsing();
        player.Animation.SetBool(IsShootingId, true);
        rig.weight = 1;
    }

    protected override void ClientUsing()
    {
        base.ClientUsing();
        audio.Play();
    }

    protected override void ServerUsing()
    {
        base.ServerUsing();
        Vector3 shootDirection = (player.Class.Target - gunTip.position).normalized;
        shootDirection.y = 0;

        MachineGunBullet bullet = Object.Instantiate(bulletPrefab, gunTip.position,
            Quaternion.LookRotation(shootDirection)).GetComponent<MachineGunBullet>();
        bullet.Initialize(OnBulletHit);
        NetworkServer.Spawn(bullet.gameObject);
    }

    private void OnBulletHit(EnemyHealth enemyHealth)
    {
        enemyHealth.TakeDamage(damages);
        player.Class.Radioactivity.Increase(radioactivityPerHit);
    }

    public override void ClientStopUsing()
    {
        base.ClientStopUsing();
        rig.weight = 0;
    }

    public override void ServerStopUsing()
    {
        base.ServerStopUsing();
        player.Animation.SetBool(IsShootingId, false);
        rig.weight = 0;
    }
}