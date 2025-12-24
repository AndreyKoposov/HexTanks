using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    public static readonly string L_GRID = "Grid";
    public static readonly string L_HIGHLIGHT = "Highlight";

    private HexTile previousTile;

    private bool HasHover
    {
        get => previousTile != null;
    }

    private void Update()
    {
        CheckHover();
        CheckSelect();
    }

    #region Main Logic
    private void CheckHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 20f, LayerMask.GetMask(L_GRID, L_HIGHLIGHT)))
        {
           var tile = hit.collider.gameObject.GetComponent<HexTile>();

            if (!HasHover || previousTile.position != tile.position)
            {
                UnhoverTile();
                previousTile = tile;
                HoverTile();
            }
        }
        else
        {
            UnhoverTile();
            previousTile = null;
        }
    }
    private void CheckSelect()
    {
        if (HasHover && Input.GetMouseButtonDown(0))
        {
            GlobalEventManager.OnUnitCreated.Invoke(Team.PLAYER, previousTile.position);
        }

        if (HasHover && Input.GetMouseButtonDown(1))
        {
            GlobalEventManager.OnUnitDestroyed.Invoke(Team.PLAYER, previousTile.position);
        }
    }
    #endregion

    #region Operations
    private void HoverTile()
    {
        previousTile.gameObject.layer = LayerMask.NameToLayer(L_HIGHLIGHT);
    }
    private void UnhoverTile()
    {
        if (HasHover)
            previousTile.gameObject.layer = LayerMask.NameToLayer(L_GRID);
    }
    #endregion
}
