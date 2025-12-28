using Codice.Client.Common.GameUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MouseInteract : MonoBehaviour
{
    public static readonly string L_GRID = "Grid";
    public static readonly string L_HIGHLIGHT = "Highlight";
    public static readonly string L_UI = "UI";

    private VectorHex hoveredTile = VectorHex.UNSIGNED;
    private VectorHex selectedTile = VectorHex.UNSIGNED;
    private readonly List<VectorHex> validMoves = new();
    private readonly List<VectorHex> validAttacks = new();
    private bool selectBuilding = false;

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
        if (Physics.Raycast(ray, out var hit, 20f, LayerMask.GetMask(L_GRID, L_HIGHLIGHT, L_UI)))
        {
           var tile = hit.collider.gameObject.GetComponent<HexTile>().Position;

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
            else
                DeselectAll();
        }
    }
    #endregion

    #region Commands
    private void UnitSelectCommand()
    {
        DeselectAll();

        SelectAllUnitTiles();
    }
    private void BuildingSelectCommand()
    {
        DeselectAll();

        SelectTile();
        SelectBuilding();
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
        if (!HoverExist) return;

        Game.World[hoveredTile].SetLayer(L_GRID);
        hoveredTile = VectorHex.UNSIGNED;
    }
    private void SelectTile()
    {
        selectedTile = hoveredTile;
        Game.World[selectedTile].ApplySelect(SelectType.Default);
    }
    private void DeselectTile()
    {
        if (!SelectExist) return;

        Game.World[selectedTile].ApplySelect(SelectType.None);
        selectedTile = VectorHex.UNSIGNED;
    }
    private void SelectValidMoves()
    {
        validMoves.AddRange(Game.World.GetValidMovesForUnit(selectedTile));

        foreach (var move in validMoves)
            Game.World[move].ApplySelect(SelectType.Default);
    }
    private void DeselectValidMoves()
    {
        if (validMoves.Count == 0) return;

        foreach (var move in validMoves)
            Game.World[move].ApplySelect(SelectType.None);

        validMoves.Clear();
    }
    private void SelectValidAttacks()
    {
        validAttacks.AddRange(Game.World.GetValidAttacksForUnit(selectedTile));

        foreach (var move in validAttacks)
            Game.World[move].ApplySelect(SelectType.Attack);
    }
    private void DeselectValidAttacks()
    {
        if (validAttacks.Count == 0) return;

        foreach (var move in validAttacks)
            Game.World[move].ApplySelect(SelectType.None);

        validAttacks.Clear();
    }
    private void SelectBuilding()
    {
        Building building = Game.World[selectedTile].Building;

        Game.UI.OpenBuildingPanel(building);

        selectBuilding = true;
    }
    private void DeselectBuilding()
    {
        if (!selectBuilding) return;

        Game.UI.CloseBuildingPanel();

        selectBuilding = false;
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
    private void DeselectAll()
    {
        DeselectAllUnitTiles();
        DeselectBuilding();
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
        if (selectedTile == unitPos)
            DeselectAllUnitTiles();
    }
    private void DeselectOnTurnChanged(Team _)
    {
        DeselectAll();
    }
    #endregion
}
