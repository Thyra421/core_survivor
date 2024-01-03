using UnityEngine;

public class MessageTarget : MessageBase
{
    public Vector3 target;
    
    public MessageTarget(Vector3 target)
    {
        this.target = target;
    }
}