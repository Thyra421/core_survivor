using System;
using System.Collections;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Blast : AbilityBase, ITargeted
{
    [SerializeField]
    private Transform handTransform;

    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private int damages = 100;

    [SerializeField]
    private float grenadeExplosionRadius = 3;

    public Vector3? Target { get; private set; }
    public override bool IsChanneled => false;

    public string Serialize(Vector3 target)
    {
        return JsonUtility.ToJson(new MessageTarget(target));
    }

    public override void ClientUse(string args)
    {
        player.Animation.SetTrigger("Throw");

        if (player.isOwned)
            Cooldown.Start();
    }

    public override void ServerUse(string args)
    {
        MessageTarget message = JsonUtility.FromJson<MessageTarget>(args);
        Target = message.target;

        Cooldown.Start();
        IsCompleted = false;

        player.StartCoroutine(ServerAttackCoroutine(message.target));
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