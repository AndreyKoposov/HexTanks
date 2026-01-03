using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Building : Obstacle
{
    public VectorHex position;
    public List<HexTile> territoryInit = new();

    [SerializeField] protected Team team;
    protected List<VectorHex> territory = new();

    private Team state;
    private int playerCounter = 0;
    private int enemyCounter = 0;

    public void Init()
    {
        foreach (var tile in territoryInit)
        {
            territory.Add(tile.Position);
            tile.OnUnitSet.AddListener(UpdateStateOnUnitEnter);
            tile.OnUnitUnset.AddListener(UpdateStateOnUnitExit);
        }

        UpdateState();
        UpdateTiles();
    }

    private void UpdateState()
    {
        if (playerCounter > 0 && enemyCounter > 0)
            state = Team.Blocked;
        else
        if (playerCounter > 0 && enemyCounter == 0)
            state = Team.Player;
        else
        if (playerCounter == 0 && enemyCounter > 0)
            state = Team.Enemy;
        else
        if (playerCounter == 0 && enemyCounter == 0)
            state = Team.Neutral;
    }

    private void UpdateTiles()
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
    private void UpdateStateOnUnitEnter(Team unitTeam)
    {
        if (unitTeam == Team.Player)
            playerCounter++;
        if (unitTeam == Team.Enemy)
            enemyCounter++;

        UpdateState();
        UpdateTiles();
    }
    private void UpdateStateOnUnitExit(Team unitTeam)
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
