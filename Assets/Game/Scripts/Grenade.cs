using System;
using System.Collections;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransformUnreliable))]
public class Grenade : NetworkBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject explosionPrefab;

    private Vector3 _destination;
    private Vector3 _origin;
    private float _t;
    private bool _hasExploded;
    private Action _onExplode;

    [ClientRpc]
    private void ExplodeRpc(Vector3 position)
    {
        Instantiate(explosionPrefab, position, quaternion.identity);
    }

    private void Update()
    {
        if (!isServer) return;
        if (_hasExploded) return;

        _t += Time.deltaTime * speed;
        transform.position = Vector3.Slerp(_origin, _destination, _t);

        if (Vector3.Distance(transform.position, _destination) > .1f) return;

        _hasExploded = true;

        _onExplode();
        ExplodeRpc(_destination);
        // delay destruction to let time to explode
        StartCoroutine(DelayedDestroyCoroutine());
    }

    private IEnumerator DelayedDestroyCoroutine()
    {
        yield return new WaitForSeconds(1);
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    public void Initialize(Vector3 destination, Action onExplode)
    {
        _destination = destination;
        _origin = transform.position;
        _onExplode = onExplode;
    }
}