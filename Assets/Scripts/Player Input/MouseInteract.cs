using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    public static readonly string L_GRID = "Grid";
    public static readonly string L_HIGHLIGHT = "Highlight";
    public static readonly string L_SELECT = "Select";

    private HexTile hoveredTile;
    private HexTile selectedTile;

    private bool HoverExist
    {
        get => hoveredTile != null;
    }
    private bool SelectExist
    {
        get => selectedTile != null;
    }

    private void Update()
    {
        CheckHover();
        CheckSelect();

        if (HoverExist && Input.GetKeyDown(KeyCode.C))
            Game.World.CreateUnit(hoveredTile.position, Team.PLAYER);
        if (HoverExist && Input.GetKeyDown(KeyCode.X))
            Game.World.DestroyUnit(hoveredTile.position);
    }

    #region Main Logic
    private void CheckHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 20f, LayerMask.GetMask(L_GRID, L_HIGHLIGHT, L_SELECT)))
        {
           var tile = hit.collider.gameObject.GetComponent<HexTile>();

            if (!HoverExist || hoveredTile.position != tile.position)
            {
                UnhighlightTile();
                hoveredTile = tile;
                HighlightTile();
            }
        }
        else
        {
            UnhighlightTile();
            hoveredTile = null;
        }
    }
    private void CheckSelect()
    {
        if (Input.GetMouseButtonDown(0) && HoverExist && hoveredTile.HasUnit)
        {
            DeselectTile();
            selectedTile = hoveredTile;
            SelectTile();
        }
    }
    #endregion

    #region Operations
    private void HighlightTile()
    {
        hoveredTile.gameObject.layer = LayerMask.NameToLayer(L_HIGHLIGHT);
    }
    private void UnhighlightTile()
    {
        if (HoverExist)
        {
            if (SelectExist && hoveredTile.position == selectedTile.position)
                hoveredTile.gameObject.layer = LayerMask.NameToLayer(L_SELECT);
            else
                hoveredTile.gameObject.layer = LayerMask.NameToLayer(L_GRID);
        }
    }
    private void SelectTile()
    {
        selectedTile.gameObject.layer = LayerMask.NameToLayer(L_SELECT);
    }
    private void DeselectTile()
    {
        if (SelectExist)
            selectedTile.gameObject.layer = LayerMask.NameToLayer(L_GRID);
    }
    #endregion
}
