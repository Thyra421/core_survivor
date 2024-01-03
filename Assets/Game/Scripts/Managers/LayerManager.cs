using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    [SerializeField]
    private LayerMask _whatIsGround;

    public LayerMask WhatIsGround => _whatIsGround;
}