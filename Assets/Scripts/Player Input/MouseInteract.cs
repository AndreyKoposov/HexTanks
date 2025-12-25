using System.Collections.Generic;
using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    public static readonly string L_GRID = "Grid";
    public static readonly string L_HIGHLIGHT = "Highlight";
    public static readonly string L_SELECT = "Select";

    private HexTile hoveredTile;
    private HexTile selectedTile;
    private List<HexTile> validMoves = new();

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
        CheckClick();

        if (HoverExist && Input.GetKeyDown(KeyCode.C))
            Game.World.CreateUnitAt(hoveredTile.position, Team.Player);
        if (HoverExist && Input.GetKeyDown(KeyCode.X))
            Game.World.DestroyUnitAt(hoveredTile.position);
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
    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0) && HoverExist)
        {
            if (hoveredTile.HasUnit)
            {
                DeselectTile();
                selectedTile = hoveredTile;
                SelectTile();
            }
            else if (SelectExist && validMoves.Contains(hoveredTile))
            {
                Game.World.MoveUnitFromTo(selectedTile.position, hoveredTile.position);
            }
        }

        if (SelectExist && !selectedTile.HasUnit)
        {
            DeselectTile();
            selectedTile = null;
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
            if (SelectExist && (validMoves.Contains(hoveredTile) || selectedTile.position == hoveredTile.position))
                hoveredTile.gameObject.layer = LayerMask.NameToLayer(L_SELECT);
            else
                hoveredTile.gameObject.layer = LayerMask.NameToLayer(L_GRID);
        }
    }
    private void SelectTile()
    {
        selectedTile.gameObject.layer = LayerMask.NameToLayer(L_SELECT);

        GetValidMoves();
        SelectValidMoves();
    }
    private void DeselectTile()
    {
        if (SelectExist)
        {
            selectedTile.gameObject.layer = LayerMask.NameToLayer(L_GRID);
            DeselectValidMoves();
        }
    }
    private void GetValidMoves()
    {
        validMoves = Game.World.GetValidMovesForUnit(selectedTile.position);
    }
    private void SelectValidMoves()
    {
        foreach (var move in validMoves)
            move.gameObject.layer = LayerMask.NameToLayer(L_SELECT);
    }
    private void DeselectValidMoves()
    {
        foreach (var move in validMoves)
            move.gameObject.layer = LayerMask.NameToLayer(L_GRID);
    }
    #endregion
}
