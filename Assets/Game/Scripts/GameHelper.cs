using UnityEngine;

public static class GameHelper
{
    public static bool GetMousePositionToWorldPoint(LayerMask whatIsAllowed, ref Vector3 position)
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main!.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 100, whatIsAllowed)) return false;

        position = hit.point;
        return true;
    }
}