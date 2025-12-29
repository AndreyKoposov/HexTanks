using System.Collections.Generic;
using UnityEngine;

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

        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha1))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Infantry, Team.Player);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha2))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Heavy, Team.Player);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha3))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Scout, Team.Player);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha4))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Artillery, Team.Player);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha5))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Defender, Team.Player);

        if (HoverExist && Input.GetKeyDown(KeyCode.F) && Game.Grid[hoveredTile].HasUnit && Game.Grid[hoveredTile].Unit is Defender)
        {
            (Game.Grid[hoveredTile].Unit as Defender).SetField();
        }

        if (HoverExist && Input.GetKeyDown(KeyCode.X))
            GlobalEventManager.UnitDied.Invoke(hoveredTile);
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
            if (Game.Grid[hoveredTile].HasUnit)
                UnitSelectCommand();
            else
            if (Game.Grid[hoveredTile].HasBuilding)
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
        Game.Grid.MoveUnitFromTo(selectedTile, hoveredTile);

        DeselectAllUnitTiles();
    }
    private void UnitAttackCommand()
    {
        Game.Grid.AttackUnitAt(selectedTile, hoveredTile);

        DeselectAllUnitTiles();
    }
    #endregion

    #region Operations
    private void HighlightTile(VectorHex newTile)
    {
        hoveredTile = newTile;
        Game.Grid[hoveredTile].SetLayer(L_HIGHLIGHT);
    }
    private void UnhighlightTile()
    {
        if (!HoverExist) return;

        Game.Grid[hoveredTile].SetLayer(L_GRID);
        hoveredTile = VectorHex.UNSIGNED;
    }
    private void SelectTile()
    {
        selectedTile = hoveredTile;
        Game.Grid[selectedTile].ApplySelect(SelectType.Default);
    }
    private void DeselectTile()
    {
        if (!SelectExist) return;

        Game.Grid[selectedTile].ApplySelect(SelectType.None);
        selectedTile = VectorHex.UNSIGNED;
    }
    private void SelectValidMoves()
    {
        validMoves.AddRange(Game.Grid.GetValidMovesForUnit(selectedTile));

        foreach (var move in validMoves)
            Game.Grid[move].ApplySelect(SelectType.Default);
    }
    private void DeselectValidMoves()
    {
        if (validMoves.Count == 0) return;

        foreach (var move in validMoves)
            Game.Grid[move].ApplySelect(SelectType.None);

        validMoves.Clear();
    }
    private void SelectValidAttacks()
    {
        validAttacks.AddRange(Game.Grid.GetValidAttacksForUnit(selectedTile));

        foreach (var move in validAttacks)
            Game.Grid[move].ApplySelect(SelectType.Attack);
    }
    private void DeselectValidAttacks()
    {
        if (validAttacks.Count == 0) return;

        foreach (var move in validAttacks)
            Game.Grid[move].ApplySelect(SelectType.None);

        validAttacks.Clear();
    }
    private void SelectBuilding()
    {
        Building building = Game.Grid[selectedTile].Building;

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
        GlobalEventManager.UnitDied.AddListener(DeselectOnUnitDied);
        GlobalEventManager.EndTurn.AddListener(DeselectOnTurnChanged);
    } 
    private void DeselectOnUnitDied(VectorHex unitPos)
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
