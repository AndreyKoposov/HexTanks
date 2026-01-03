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

        GlobalEventManager.TurnChanged.AddListener(ProgressBuildOnTurnChanged);
    }

    #region Events
    private void ProgressBuildOnTurnChanged(int _)
    {
        if (state == Team.Player && Game.Player.unitsHas >= Game.Player.unitsMax)
        {
            GlobalEventManager.TurnChanged.RemoveListener(ProgressBuildOnTurnChanged);
            return;
        }
        if (state == Team.Enemy && Game.Enemy.unitsHas >= Game.Enemy.unitsMax)
        {
            GlobalEventManager.TurnChanged.RemoveListener(ProgressBuildOnTurnChanged);
            return;
        }

        turnsLeft--;

        if (turnsLeft <= 0)
        {
            Game.Fabric.CreateUnitAt(position, unitToBuild, state);

            GlobalEventManager.TurnChanged.RemoveListener(ProgressBuildOnTurnChanged);
        }
    }
    #endregion
}
