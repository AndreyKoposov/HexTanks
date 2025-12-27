using System.Collections.Generic;
using UnityEngine;

public class Building : Obstacle
{
    public List<VectorHex> territory = new();

    [SerializeField] private Team team;

    public VectorHex position;
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
            Game.World.CreateUnitAt(position, unitToBuild, team);

        GlobalEventManager.TurnChanged.RemoveListener(ProgressBuildOnTurnChanged);
    }
    #endregion
}
