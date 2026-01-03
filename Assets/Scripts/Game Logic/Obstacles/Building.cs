using System.Collections.Generic;
using UnityEngine;

public class Building : Obstacle
{
    public List<HexTile> territoryInit = new();

    [SerializeField] protected BuildingInfo info;

    protected VectorHex position;
    protected List<VectorHex> territory = new();
    protected Team state;
    protected int playerCounter = 0;
    protected int enemyCounter = 0;

    private void Start()
    {
        SetState(state);
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

    protected void SetState(Team state)
    {
        if (state == Team.Player)
            GetComponent<MeshRenderer>().material = Game.Art.PlayerMat;
        else
        if (state == Team.Enemy)
            GetComponent<MeshRenderer>().material = Game.Art.EnemyMat;
        else
            GetComponent<MeshRenderer>().material = Game.Art.NeutralMat;
    }

    protected virtual void UpdateState()
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

        SetState(state);
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
