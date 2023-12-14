using UnityEngine;

public partial class Player
{
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Camera.main!.GetComponent<CameraController>().SetTarget(transform);
    }
}
