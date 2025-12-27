using System.Collections.Generic;
using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    public static readonly string L_GRID = "Grid";
    public static readonly string L_HIGHLIGHT = "Highlight";
    public static readonly string L_SELECT = "Select";
    public static readonly string L_ATTACK = "Attack";

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

    private void Awake()
    {
        RegisterOnEvents();
    }

    private void Update()
    {
        CheckHover();
        CheckClick();

        if (HoverExist && Input.GetKeyDown(KeyCode.C))
            Game.World.CreateUnitAt(hoveredTile, UnitType.Tank, Team.Player);
        if (HoverExist && Input.GetKeyDown(KeyCode.X))
            Game.World.DestroyUnitAt(hoveredTile);
    }

    #region Main Logic
    private void CheckHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 20f, LayerMask.GetMask(L_GRID, L_HIGHLIGHT, L_SELECT, L_ATTACK)))
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
            else
            if (Game.World[hoveredTile].HasBuilding)
                BuildingSelectCommand();
        }
    }
    #endregion

    #region Commands
    private void UnitSelectCommand()
    {
        DeselectAllUnitTiles();
        SelectAllUnitTiles();
    }
    private void BuildingSelectCommand()
    {
        DeselectTile();
        SelectTile();

        Game.World.SelectBuildingAt(selectedTile);
    }
    private void UnitMoveCommand()
    {
        Game.World.MoveUnitFromTo(selectedTile, hoveredTile);

        DeselectAllUnitTiles();
    }
    private void UnitAttackCommand()
    {
        Game.World.AttackUnitAt(selectedTile, hoveredTile);

        DeselectAllUnitTiles();
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
            if (SelectExist)
                if (validMoves.Contains(hoveredTile) || selectedTile == hoveredTile)
                    Game.World[hoveredTile].SetLayer(L_SELECT);
                else 
                if (validAttacks.Contains(hoveredTile))
                    Game.World[hoveredTile].SetLayer(L_ATTACK);
                else
                    Game.World[hoveredTile].SetLayer(L_GRID);
            else
                Game.World[hoveredTile].SetLayer(L_GRID);

            hoveredTile = VectorHex.UNSIGNED;
        }
    }
    private void SelectTile()
    {
        selectedTile = hoveredTile;
        Game.World[selectedTile].SetLayer(L_SELECT);
    }
    private void DeselectTile()
    {
        if (SelectExist)
        {
            Game.World[selectedTile].SetLayer(L_GRID);
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
    private void SelectValidAttacks()
    {
        validAttacks.AddRange(Game.World.GetValidAttacksForUnit(selectedTile));

        foreach (var move in validAttacks)
            Game.World[move].SetLayer(L_ATTACK);
    }
    private void DeselectValidAttacks()
    {
        foreach (var move in validAttacks)
            Game.World[move].SetLayer(L_GRID);

        validAttacks.Clear();
    }
    private void SelectAllUnitTiles()
    {
        SelectTile();
        SelectValidMoves();
        SelectValidAttacks();
    }
    private void DeselectAllUnitTiles()
    {
        DeselectTile();
        DeselectValidMoves();
        DeselectValidAttacks();
    }
    #endregion

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.UnitDestroyed.AddListener(DeselectOnUnitDestroy);
        GlobalEventManager.EndTurn.AddListener(DeselectOnTurnChanged);
    } 
    private void DeselectOnUnitDestroy(Vector3Int unitPos)
    {
        if (SelectExist && selectedTile == unitPos)
        {
            DeselectAllUnitTiles();
        }
    }
    private void DeselectOnTurnChanged(Team _)
    {
        if (SelectExist)
        {
            DeselectAllUnitTiles();
        }
    }
    #endregion
}
