using System;
using System.Collections;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Blast : InstantAbility
{
    [SerializeField]
    private Transform handTransform;

    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private float grenadeExplosionRadius = 3;

    public override void ClientUse()
    {
        base.ClientUse();
        player.Animation.SetTrigger("Throw");
        Cooldown.Start();
    }

    public override void ServerUse()
    {
        base.ServerUse();
        player.StartCoroutine(ServerAttackCoroutine(player.Class.Target));
    }

    private IEnumerator ServerAttackCoroutine(Vector3 target)
    {
        yield return new WaitForSeconds(delay);

        Grenade grenade = Object.Instantiate(grenadePrefab, handTransform.position, Quaternion.identity)
            .GetComponent<Grenade>();

        grenade.Initialize(target, () => OnGrenadeExplode(target));
        NetworkServer.Spawn(grenade.gameObject);
    }

    private void OnGrenadeExplode(Vector3 target)
    {
        Collider[] hits = Physics.OverlapSphere(target, grenadeExplosionRadius);

        if (hits.Length <= 0) return;

        foreach (Collider c in hits) {
            if (!c.transform.TryGetComponent(out EnemyHealth enemy)) continue;

            // check that enemy is in line of sight
            Vector3 enemyPosition = c.transform.position + Vector3.up;
            Vector3 grenadePosition = target + Vector3.up;
            Ray enemyRay = new(grenadePosition, enemyPosition - grenadePosition);
            if (Physics.Raycast(enemyRay, Vector3.Distance(enemyPosition, grenadePosition),
                    LayerManager.Current.WhatIsObstacle)) continue;
            enemy.TakeDamage(damages);
        }
    }
}