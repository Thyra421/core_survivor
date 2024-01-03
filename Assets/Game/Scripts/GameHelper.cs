using UnityEngine;

public static class GameHelper
{
    public static Vector3? GetMousePositionToWorldPoint(LayerMask whatIsAllowed)
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main!.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100, whatIsAllowed)) return null;

        return hit.point;
    }
}