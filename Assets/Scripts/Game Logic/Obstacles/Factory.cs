using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    private UnitType unitToBuild;
    private int turnsLeft;

    public UnitType UnitToBuild => unitToBuild;
    public int TurnsLeft => turnsLeft;

    public void StartBuildUnit(UnitType type)
    {
        unitToBuild = type;
        turnsLeft = 2;

        Game.GetPlayer(state).SubstactResourcesForUnit(unitToBuild);
        GlobalEventManager.EndTurn.AddListener(ProgressBuildOnTurnChanged);
    }

    #region Events
    private void ProgressBuildOnTurnChanged(Team nextPlayer)
    {
        if (state == nextPlayer) return;

        PlayerData player = Game.GetPlayer(state);
        if (player.LimitReached)
        {
            Game.GetPlayer(state).AddResourcesForUnit(unitToBuild);
            GlobalEventManager.EndTurn.RemoveListener(ProgressBuildOnTurnChanged);

            return;
        }

        turnsLeft--;

        if (turnsLeft <= 0)
        {
            Game.Fabric.CreateUnitAt(position, unitToBuild, state);

            GlobalEventManager.EndTurn.RemoveListener(ProgressBuildOnTurnChanged);
        }
    }
    #endregion
}
