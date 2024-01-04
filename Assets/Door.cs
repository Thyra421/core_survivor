using System;
using System.Linq;
using Mirror;
using UnityEngine;

public class Door : NetworkBehaviour
{
    [SerializeField]
    private Transform leftDoor;

    [SerializeField]
    private Transform rightDoor;

    [SerializeField]
    private float doorGap = 1;

    [SerializeField]
    private float distanceThreshold = 5;

    [SerializeField]
    private float openingSpeed = 10;

    private bool _isOpen;
    private Vector3 _rightDoorInitialPosition;
    private Vector3 _leftDoorInitialPosition;

    private float _t;

    private void Awake()
    {
        _rightDoorInitialPosition = rightDoor.transform.position;
        _leftDoorInitialPosition = leftDoor.transform.position;
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

        rightDoor.transform.position = Vector3.Lerp(_rightDoorInitialPosition,
            _rightDoorInitialPosition + Vector3.right * doorGap, _t);
        leftDoor.transform.position = Vector3.Lerp(_leftDoorInitialPosition,
            _leftDoorInitialPosition + Vector3.left * doorGap, _t);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceThreshold);
    }
}