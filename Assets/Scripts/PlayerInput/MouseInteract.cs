using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    public static readonly string L_GRID = "Grid";
    public static readonly string L_HIGHLIGHT = "Highlight";

    private HexTile hoveredTile;

    private void Update()
    {
        CheckHover();
        CheckSelect();
    }

    private bool HasHover
    {
        get => hoveredTile != null;
    }

    private void CheckHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 20f, LayerMask.GetMask(L_GRID, L_HIGHLIGHT)))
        {
            var tile = hit.collider.gameObject.GetComponent<HexTile>();

            if (!HasHover || hoveredTile.position != tile.position)
            {
                UnhoverTile();
                hoveredTile = tile;
                HoverTile();
            }
        }
        else
        {
            UnhoverTile();
            hoveredTile = null;
        }
    }
    private void CheckSelect()
    {
    }

    private void HoverTile()
    {
        hoveredTile.gameObject.layer = LayerMask.NameToLayer(L_HIGHLIGHT);
    }

    private void UnhoverTile()
    {
        if (HasHover)
            hoveredTile.gameObject.layer = LayerMask.NameToLayer(L_GRID);
    }
}
