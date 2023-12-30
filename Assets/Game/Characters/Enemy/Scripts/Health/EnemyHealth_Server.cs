using System.Collections;
using Mirror;
using UnityEngine;

public partial class EnemyHealth
{
    [Server]
    public override void Die()
    {
        ServerDie();
        ClientDie();
    }

    [Server]
    private void ServerDie()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(8);
        NetworkServer.Destroy(gameObject);
    }
}