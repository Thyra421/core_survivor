using UnityEngine;

public partial class Player
{
    [SerializeField]
    private Material[] materials;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Camera.main!.GetComponent<CameraController>().SetTarget(transform);
        PlayerManager.Current.LocalPlayer.Value = this;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        renderer.material = materials[Index];
    }
}