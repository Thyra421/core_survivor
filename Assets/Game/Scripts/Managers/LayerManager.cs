using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    [SerializeField]
    private LayerMask _whatIsGround;
    
    [SerializeField]
    private LayerMask _whatIsObstacle;

    public LayerMask WhatIsGround => _whatIsGround;
    
    public LayerMask WhatIsObstacle => _whatIsObstacle;
}