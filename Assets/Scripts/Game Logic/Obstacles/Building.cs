using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Obstacle
{
    public VectorHex position;
    public List<VectorHex> territory = new();

    [SerializeField] protected Team team;

    private Team state;
    private int playerCounter = 0;
    private int enemyCounter = 0;

    private void Start()
    {
        foreach (var pos in territory)
        {
            Game.Grid[pos].OnUnitSet.AddListener(UpdateStateOnUnitEnter);
            Game.Grid[pos].OnUnitUnset.AddListener(UpdateStateOnUnitExit);
        }
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
            state = Team.Neutral;
    }

    #region Events
    private void UpdateStateOnUnitEnter(Team unitTeam)
    {
        if (unitTeam == Team.Player)
            playerCounter++;
        if (unitTeam == Team.Enemy)
            enemyCounter++;

        UpdateState();
    }
    private void UpdateStateOnUnitExit(Team unitTeam)
    {
        if (unitTeam == Team.Player)
            playerCounter--;
        if (unitTeam == Team.Enemy)
            enemyCounter--;

        UpdateState();
    }
    #endregion
}
