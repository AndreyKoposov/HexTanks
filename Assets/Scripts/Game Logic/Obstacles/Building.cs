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
    protected virtual Team Default => Team.Neutral;

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

        UpdateAll();
    }

    protected void UpdateState()
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
            newState = Default;

        if (newState != state)
        {
            if (state != Team.Neutral && state != Team.Blocked)
                Game.GetPlayer(state).RemoveBuilding(info);
            if (newState != Team.Neutral && newState != Team.Blocked)
                Game.GetPlayer(newState).AddBuilding(info);
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
    protected void UpdateMaterial()
    {
        if (state == Team.Player)
            GetComponent<MeshRenderer>().material = Game.Art.PlayerMat;
        else
        if (state == Team.Enemy)
            GetComponent<MeshRenderer>().material = Game.Art.EnemyMat;
        else
            GetComponent<MeshRenderer>().material = Game.Art.NeutralMat;
    }
    private void UpdateAll()
    {
        UpdateState();
        UpdateTiles();
        UpdateMaterial();
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.EndTurn.AddListener(ProduceOnEndTurn);
    }
    private void ProduceOnEndTurn(Team nextPlayer)
    {
        if (state == nextPlayer)
            Game.GetPlayer(state).AddResourcesForBuilding(info);
    }
    protected void UpdateStateOnUnitEnter(Team unitTeam)
    {
        if (unitTeam == Team.Player)
            playerCounter++;
        if (unitTeam == Team.Enemy)
            enemyCounter++;

        UpdateAll();
    }
    protected void UpdateStateOnUnitExit(Team unitTeam)
    {
        if (unitTeam == Team.Player)
            playerCounter--;
        if (unitTeam == Team.Enemy)
            enemyCounter--;

        UpdateAll();
    }
    #endregion
}
