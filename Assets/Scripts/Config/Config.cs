using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Config")]
public class Config : SingletonScriptableObject<Config>
{
    [FormerlySerializedAs("tcpChunkSize")] [Header("Network")] [SerializeField] private int messageChunkSize = 1024;

    public int MessageChunkSize => messageChunkSize;
}