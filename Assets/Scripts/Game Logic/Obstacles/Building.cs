using System.Collections.Generic;
using UnityEngine;

public class Building : Obstacle
{
    public List<HexTile> territoryInit = new();

    [SerializeField] protected BuildingInfo info;

    protected VectorHex position;
    protected List<VectorHex> territory = new();
    protected Team state = Team.Neutral;
    protected int playerCounter = 0;
    protected int enemyCounter = 0;

    public Team Team => state;

    private void Awake()
    {
        RegisterOnEvents();
    }

    public void Init(VectorHex pos)
    {
        position = pos;

        foreach (var tile in territoryInit)
        {
            territory.Add(tile.Position);
            tile.OnUnitSet.AddListener(UpdateStateOnUnitEnter);
            tile.OnUnitUnset.AddListener(UpdateStateOnUnitExit);
        }

        UpdateState();
        UpdateTiles();
    }

    protected void SetMaterial(Team newState)
    {
        if (newState == Team.Player)
            GetComponent<MeshRenderer>().material = Game.Art.PlayerMat;
        else
        if (newState == Team.Enemy)
            GetComponent<MeshRenderer>().material = Game.Art.EnemyMat;
        else
            GetComponent<MeshRenderer>().material = Game.Art.NeutralMat;
    }

    protected virtual void UpdateState()
    {
        Team newState;

        if (playerCounter > 0 && enemyCounter > 0)
            newState = Team.Blocked;
        else
        if (playerCounter > 0 && enemyCounter == 0)
            newState = Team.Player;
        else
        if (playerCounter == 0 && enemyCounter > 0)
            newState = Team.Enemy;
        else
            newState = Team.Neutral;

        SetMaterial(newState);

        if (newState != state)
        {
            if (state != Team.Neutral && state != Team.Blocked)
                GlobalEventManager.PlayerLoseBuilding.Invoke(state, info);
            if (newState != Team.Neutral && newState != Team.Blocked)
                GlobalEventManager.PlayerGotBuilding.Invoke(newState, info);
        }

        state = newState;
    }

    protected void UpdateTiles()
    {
        foreach (var pos in territory)
        {
            List<HexDirections> dirs = new();

            if (!territory.Contains(pos + pos.Left))
                dirs.Add(HexDirections.Left);
            if (!territory.Contains(pos + pos.LeftTop))
                dirs.Add(HexDirections.LeftTop);
            if (!territory.Contains(pos + pos.RightTop))
                dirs.Add(HexDirections.RightTop);
            if (!territory.Contains(pos + pos.Right))
                dirs.Add(HexDirections.Right);
            if (!territory.Contains(pos + pos.RightBottom))
                dirs.Add(HexDirections.RightBottom);
            if (!territory.Contains(pos + pos.LeftBottom))
                dirs.Add(HexDirections.LeftBottom);

            Game.Grid[pos].SetTerritory(state, dirs);
        }
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.TurnChanged.AddListener(ProduceOnTurnChanged);
    }
    private void ProduceOnTurnChanged(int _)
    {
        PlayerData player = state == Team.Player ? Game.Player : Game.Enemy;

        player.plasm += info.ProducePlasm;
        player.titan += info.ProduceTitan;
        player.chips += info.ProduceChips;

        Game.UI.UpdatePlayerPanel(player);
    }
    protected void UpdateStateOnUnitEnter(Team unitTeam)
    {
        if (unitTeam == Team.Player)
            playerCounter++;
        if (unitTeam == Team.Enemy)
            enemyCounter++;

        UpdateState();
        UpdateTiles();
    }
    protected void UpdateStateOnUnitExit(Team unitTeam)
    {
        if (unitTeam == Team.Player)
            playerCounter--;
        if (unitTeam == Team.Enemy)
            enemyCounter--;

        UpdateState();
        UpdateTiles();
    }
    #endregion
}
