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
    private bool selectDefender = false;

    private bool selectTransport = false;
    private int indexToUnboard = -1;
    private Transport transport;

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

        Team team = Game.CurrentTeam;

        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha1))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Infantry, team);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha2))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Heavy, team);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha3))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Scout, team);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha4))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Artillery, team);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha5))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Defender, team);
        if (HoverExist && Input.GetKeyDown(KeyCode.Alpha6))
            Game.Fabric.CreateUnitAt(hoveredTile, UnitType.Transport, team);

        //if (HoverExist && Input.GetKeyDown(KeyCode.X))
        //    GlobalEventManager.UnitDied.Invoke(hoveredTile);
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

        if (Game.Grid[selectedTile].Unit.Info.Type == UnitType.Defender)
        {
            selectDefender = true;
            Game.UI.ShowDefenderButton(Game.Grid[selectedTile].Unit as Defender);
        }
        else
        if (Game.Grid[selectedTile].Unit.Info.Type == UnitType.Transport)
        {
            selectTransport = true;
            Game.UI.OpenTrasportPanel(Game.Grid[selectedTile].Unit as Transport);
        }
    }
    private void BuildingSelectCommand()
    {
        DeselectAll();

        SelectTile();
        SelectBuilding();
    }
    private void UnitMoveCommand()
    {
        if (indexToUnboard >= 0)
            Game.Grid.UnboardUnitFromTo(transport.Position, hoveredTile, indexToUnboard);
        else
            Game.Grid.MoveUnitFromTo(selectedTile, hoveredTile);

        DeselectAll();
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
        validMoves.AddRange(Game.Grid.GetValidMoves(selectedTile));

        foreach (var move in validMoves)
            Game.Grid[move].ApplySelect(SelectType.Default);
    }
    private void SelectValidUnboardMoves(Transport transport, int index)
    {
        validMoves.AddRange(Game.Grid.GetValidMoves(transport, index));

        validMoves.Remove(transport.Position);
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
        Factory building = Game.Grid[selectedTile].Building;

        Game.UI.OpenBuildingPanel(building);

        selectBuilding = true;
    }
    private void DeselectBuilding()
    {
        if (!selectBuilding) return;

        Game.UI.CloseBuildingPanel();

        selectBuilding = false;
    }
    private void DeselectDefender()
    {
        if (!selectDefender) return;

        Game.UI.HideDefenderButton();

        selectDefender = false;
    }
    private void DeselectTransport()
    {
        if (!selectTransport) return;

        Game.UI.CloseTransportPanel();

        selectTransport = false;
        indexToUnboard = -1;
        transport = null;
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
        DeselectDefender();
        DeselectTransport();
    }
    #endregion

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.UnitDied.AddListener(DeselectOnUnitDied);
        GlobalEventManager.EndTurn.AddListener(DeselectOnTurnChanged);
        GlobalEventManager.BoardUnitSelected.AddListener(OnBoardSelected);
    } 
    private void DeselectOnUnitDied(VectorHex unitPos, Team _)
    {
        if (selectedTile == unitPos)
            DeselectAllUnitTiles();
    }
    private void DeselectOnTurnChanged(Team _)
    {
        DeselectAll();
    }
    private void OnBoardSelected(Transport transport, int index)
    {
        DeselectAllUnitTiles();

        indexToUnboard = index;
        this.transport = transport;
        SelectValidUnboardMoves(transport, index);
    }
    #endregion
}
