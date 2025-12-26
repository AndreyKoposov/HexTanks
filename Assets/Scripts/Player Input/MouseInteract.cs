using System.Collections.Generic;
using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    public static readonly string L_GRID = "Grid";
    public static readonly string L_HIGHLIGHT = "Highlight";
    public static readonly string L_SELECT = "Select";

    private VectorHex hoveredTile = VectorHex.UNSIGNED;
    private VectorHex selectedTile = VectorHex.UNSIGNED;
    private readonly List<VectorHex> validMoves = new();
    private readonly List<VectorHex> validAttacks = new();

    private bool HoverExist
    {
        get => hoveredTile != VectorHex.UNSIGNED;
    }
    private bool SelectExist
    {
        get => selectedTile != VectorHex.UNSIGNED;
    }

    private void Start()
    {
        RegisterOnEvents();
    }

    private void Update()
    {
        CheckHover();
        CheckClick();

        if (HoverExist && Input.GetKeyDown(KeyCode.C))
            Game.World.CreateUnitAt(hoveredTile);
        if (HoverExist && Input.GetKeyDown(KeyCode.X))
            Game.World.DestroyUnitAt(hoveredTile);
    }

    #region Main Logic
    private void CheckHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 20f, LayerMask.GetMask(L_GRID, L_HIGHLIGHT, L_SELECT)))
        {
           var tile = hit.collider.gameObject.GetComponent<HexTile>().position;

            if (!HoverExist || hoveredTile != tile)
            {
                UnhighlightTile();
                HighlightTile(tile);
            }
        }
        else
            UnhighlightTile();
    }
    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0) && HoverExist)
        {
            if (validMoves.Contains(hoveredTile))
                UnitMoveCommand();
            else
            if (validAttacks.Contains(hoveredTile))
                UnitAttackCommand();
            else
            if (Game.World[hoveredTile].HasUnit)
                UnitSelectCommand();
        }
    }
    #endregion

    #region Commands
    private void UnitSelectCommand()
    {
        DeselectTile();
        SelectTile();
    }
    private void UnitMoveCommand()
    {
        Game.World.MoveUnitFromTo(selectedTile, hoveredTile);
        DeselectTile();
    }
    private void UnitAttackCommand()
    {

    }
    #endregion

    #region Operations
    private void HighlightTile(VectorHex newTile)
    {
        hoveredTile = newTile;
        Game.World[hoveredTile].SetLayer(L_HIGHLIGHT);
    }
    private void UnhighlightTile()
    {
        if (HoverExist)
        {
            if (SelectExist && (validMoves.Contains(hoveredTile) || selectedTile == hoveredTile))
                Game.World[hoveredTile].SetLayer(L_SELECT);
            else
                Game.World[hoveredTile].SetLayer(L_GRID);
        }

        hoveredTile = VectorHex.UNSIGNED;
    }
    private void SelectTile()
    {
        selectedTile = hoveredTile;
        Game.World[selectedTile].SetLayer(L_SELECT);

        SelectValidMoves();
    }
    private void DeselectTile()
    {
        if (SelectExist)
        {
            Game.World[selectedTile].SetLayer(L_GRID);
            DeselectValidMoves();
        }

        selectedTile = VectorHex.UNSIGNED;
    }
    private void SelectValidMoves()
    {
        validMoves.AddRange(Game.World.GetValidMovesForUnit(selectedTile));

        foreach (var move in validMoves)
            Game.World[move].SetLayer(L_SELECT);
    }
    private void DeselectValidMoves()
    {
        foreach (var move in validMoves)
            Game.World[move].SetLayer(L_GRID);

        validMoves.Clear();
    }
    #endregion

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.OnUnitDestroyed.AddListener(DeselectOnUnitDestroy);
    }
    private void DeselectOnUnitDestroy(Vector3Int unitPos)
    {
        if (SelectExist && selectedTile == unitPos)
        {
            DeselectTile();
        }
    }
    #endregion
}
