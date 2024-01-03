using System.Collections;
using Mirror;
using UnityEngine;

public class MachineGunBullet : NetworkBehaviour
{
    [SerializeField]
    private float speed;

    private int _damages;

    public void Initialize(int damages)
    {
        _damages = damages;
    }

    private void Start()
    {
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
            enemyHealth.TakeDamage(_damages);

        StopAllCoroutines();
        NetworkServer.Destroy(gameObject);
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2);
        NetworkServer.Destroy(gameObject);
    }
}