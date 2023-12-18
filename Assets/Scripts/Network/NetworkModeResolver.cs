using UnityEngine;

[RequireComponent(typeof(Network))]
public class NetworkModeResolver : MonoBehaviour
{
    [SerializeField] private bool useMasterPort;
    private Network _network;

    private void Awake()
    {
        _network = GetComponent<Network>();
    }

    private void Start()
    {
        _network.Initialize(useMasterPort);
    }
}