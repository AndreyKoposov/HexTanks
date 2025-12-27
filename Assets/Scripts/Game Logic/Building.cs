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

    private void Start()
    {
        if (team == Team.Player)
            GetComponent<MeshRenderer>().material = Game.Instance.playerMat;
        if (team == Team.Enemy)
            GetComponent<MeshRenderer>().material = Game.Instance.enemyMat;
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
            Game.World.CreateUnitAt(position, unitToBuild, team);

        GlobalEventManager.TurnChanged.RemoveListener(ProgressBuildOnTurnChanged);
    }
    #endregion
}
