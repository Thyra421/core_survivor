using Mirror;
using UnityEngine;

public class MachineGunBullet : NetworkBehaviour
{
    [SerializeField]
    private float speed;

    private void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }
}