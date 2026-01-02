using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    private UnitType unitToBuild;
    private int turnsLeft;

    public UnitType UnitToBuild => unitToBuild;
    public int TurnsLeft => turnsLeft;

    private void Start()
    {
        if (team == Team.Player)
            GetComponent<MeshRenderer>().material = Game.Art.PlayerMat;
        if (team == Team.Enemy)
            GetComponent<MeshRenderer>().material = Game.Art.EnemyMat;
    }

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
            Game.Fabric.CreateUnitAt(position, unitToBuild, team);

        GlobalEventManager.TurnChanged.RemoveListener(ProgressBuildOnTurnChanged);
    }
    #endregion
}
