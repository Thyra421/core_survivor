using UnityEngine;

[CreateAssetMenu(menuName = "Config")]
public class Config : SingletonScriptableObject<Config>
{
    [Header("Network")] [SerializeField] private int tcpChunkSize = 1024;

    public int TCPChunkSize => tcpChunkSize;
}