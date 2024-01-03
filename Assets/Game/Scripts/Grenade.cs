using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector3 _destination;
    private Vector3 _origin;
    private float _t;

    private void Update()
    {
        _t += Time.deltaTime * speed;
        transform.position = Vector3.Slerp(_origin, _destination, _t);
    }

    public void Initialize(Vector3 destination)
    {
        _destination = destination;
        _origin = transform.position;
    }
}