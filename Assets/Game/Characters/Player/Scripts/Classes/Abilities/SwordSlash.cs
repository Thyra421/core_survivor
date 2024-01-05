using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SwordSlash : InstantAbility
{
    [SerializeField]
    private float radius = 1.5f;

    [SerializeField]
    private float distance = 1.5f;

    [SerializeField]
    private int radioactivityPerHit;

    public override void ClientUse()
    {
        base.ClientUse();
        player.Animation.SetTrigger("Attack");
    }

    public override void ServerUse()
    {
        base.ServerUse();
        player.StartCoroutine(ServerAttackCoroutine());
    }

    private IEnumerator ServerAttackCoroutine()
    {
        yield return new WaitForSeconds(delay);

        Vector3 attackPosition =
            player.transform.position + player.transform.up + player.transform.forward * distance;
        Collider[] hits = Physics.OverlapSphere(attackPosition, radius);

        if (hits.Length <= 0) yield return null;

        int cpt = 0;

        foreach (Collider c in hits) {
            if (!c.transform.TryGetComponent(out EnemyHealth enemy)) continue;

            // check that enemy is in line of sight
            Vector3 enemyPosition = c.transform.position + Vector3.up;
            Vector3 playerPosition = player.transform.position + Vector3.up;
            Ray enemyRay = new(playerPosition, enemyPosition - playerPosition);
            if (Physics.Raycast(enemyRay, Vector3.Distance(enemyPosition, playerPosition),
                    LayerManager.Current.WhatIsObstacle)) continue;
            enemy.TakeDamage(damages);
            cpt++;
        }

        player.Class.Radioactivity.Increase(cpt * radioactivityPerHit);
    }
}