using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class MachineGunBullet : NetworkBehaviour
{
    [SerializeField]
    private float speed;

    private Action<EnemyHealth> _onHit;

    public void Initialize(Action<EnemyHealth> onHit)
    {
        _onHit = onHit;
    }

    private void Start()
    {
        if (isServer)
            StartCoroutine(DestroyCoroutine());
    }

    private void Update()
    {
        // translation is handled locally since we don't need precise positions for the clients
        // it avoids using network transform and saves performance
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;

        if (other.TryGetComponent(out EnemyHealth enemyHealth))
            _onHit(enemyHealth);

        StopAllCoroutines();
        NetworkServer.Destroy(gameObject);
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2);
        NetworkServer.Destroy(gameObject);
    }
}