using System.Collections.Generic;
using UnityEngine;

public class Building : Obstacle
{
    public List<VectorHex> territory = new();

    [SerializeField] private Team team;

    private VectorHex position;
    private UnitType unitToBuild;
    private int turnsLeft;

    public UnitType UnitToBuild => unitToBuild;
    public int TurnsLeft => turnsLeft;

    public void Setup(HexTile tile)
    {
        transform.rotation = Quaternion.identity;
        position = tile.position;

        gameObject.transform.parent = tile.gameObject.transform;
        transform.localPosition = Vector3.zero - Vector3.forward * 0.09f;
    }

    public void StartBuildUnit(UnitType unit)
    {
        unitToBuild = unit;
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
