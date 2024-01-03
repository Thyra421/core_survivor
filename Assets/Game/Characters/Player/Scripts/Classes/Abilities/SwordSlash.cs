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

    [HideInInspector]
    public Player player;

    public Vector3? Target { get; private set; }

    private Vector3 AttackPosition =>
        player.transform.position + player.transform.up + player.transform.forward * distance;

    public string Serialize(Vector3 target)
    {
        return JsonUtility.ToJson(new MessageSwordSlash { target = target });
    }

    public override void ClientUse(string args)
    {
        player.Animation.SetTrigger("Attack");

        if (!player.isOwned) return;

        Cooldown.Start();
    }

    public override void ServerUse(string args)
    {
        MessageSwordSlash message = JsonUtility.FromJson<MessageSwordSlash>(args);
        Target = message.target;

        Cooldown.Start();
        IsCompleted = false;

        player.StartCoroutine(ServerAttackCoroutine());
        player.StartCoroutine(ResetIsCompletedCoroutine());
    }

    private IEnumerator ResetIsCompletedCoroutine()
    {
        yield return new WaitForSeconds(abilityDuration);

        IsCompleted = true;
        Target = null;
    }

    private IEnumerator ServerAttackCoroutine()
    {
        yield return new WaitForSeconds(delay);

        Collider[] hits = Physics.OverlapSphere(AttackPosition, radius);

        if (hits.Length <= 0) yield return null;

        int cpt = 0;

        foreach (Collider c in hits) {
            if (!c.transform.TryGetComponent(out EnemyHealth enemy)) continue;

            enemy.TakeDamage(damages);
            cpt++;
        }

        ((IRadioactivityUser)player.Class).Radioactivity.Increase(cpt);
    }
}