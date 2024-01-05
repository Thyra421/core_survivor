using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SwordSlash : AbilityBase, ITargeted
{
    [SerializeField]
    private int damages = 25;

    [SerializeField]
    private float radius = 1.5f;

    [SerializeField]
    private float distance = 1.5f;

    public Vector3? Target { get; private set; }
    public override bool IsChanneled => false;

    public string Serialize(Vector3 target)
    {
        return JsonUtility.ToJson(new MessageTarget(target));
    }

    public override void ClientUse(string args)
    {
        player.Animation.SetTrigger("Attack");

        if (player.isOwned)
            Cooldown.Start();
    }

    public override void ServerUse(string args)
    {
        MessageTarget message = JsonUtility.FromJson<MessageTarget>(args);
        Target = message.target;

        Cooldown.Start();
        IsCompleted = false;

        player.StartCoroutine(ServerAttackCoroutine());
        player.StartCoroutine(ResetIsCompletedCoroutine());
    }

    public override void ClientEnd(string args) { }

    public override void ServerEnd(string args) { }

    private IEnumerator ResetIsCompletedCoroutine()
    {
        yield return new WaitForSeconds(abilityDuration);

        IsCompleted = true;
        Target = null;
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

        player.Class.Radioactivity.Increase(cpt * 2);
    }
}