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
        turnsLeft = 1;

        GlobalEventManager.TurnChanged.AddListener(ProgressBuildOnTurnChanged);
    }

    #region Events
    private void ProgressBuildOnTurnChanged(int _)
    {
        turnsLeft--;

        if (turnsLeft <= 0)
            Game.Fabric.CreateUnitAt(position, unitToBuild, state);

        GlobalEventManager.TurnChanged.RemoveListener(ProgressBuildOnTurnChanged);
    }
    #endregion
}
