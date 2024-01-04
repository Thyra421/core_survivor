using System.Linq;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
public class Gate : NetworkBehaviour
{
    [SerializeField]
    private Transform door;

    [SerializeField]
    private float doorGap = 2;

    [SerializeField]
    private float distanceThreshold = 3;

    [SerializeField]
    private float openingSpeed = 10;

    private bool _isOpen;
    private Vector3 _doorInitialPosition;

    private float _t;

    private void Awake()
    {
        _doorInitialPosition = door.transform.position;
    }

    private void Update()
    {
        if (!isServer)
            return;

        bool isOpening = PlayerManager.Current.Players.Any(p =>
            Vector3.Distance(p.transform.position, transform.position) < distanceThreshold);

        _t = isOpening
            ? Mathf.Clamp(_t + Time.deltaTime * openingSpeed, 0, 1)
            : Mathf.Clamp(_t - Time.deltaTime * openingSpeed, 0, 1);

        door.transform.position = Vector3.Lerp(_doorInitialPosition,
            _doorInitialPosition + Vector3.down * doorGap, _t);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceThreshold);
    }
}