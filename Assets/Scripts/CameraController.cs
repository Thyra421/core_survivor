using JetBrains.Annotations;

using UnityEngine;
public class CameraController : MonoBehaviour
{
    [CanBeNull] private Transform _target;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        if (_target == null) return;

        transform.position = _target.position + offset;
        transform.LookAt(_target);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}