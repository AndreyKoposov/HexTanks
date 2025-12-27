using System.Collections.Generic;
using UnityEngine;
using static PlasticGui.WorkspaceWindow.Merge.MergeInProgress;

public class Building : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private List<VectorHex> territory = new();

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

        GlobalEventManager.OnNextTurn.AddListener(ProgressBuildOnTurnChanged);
    }

    #region Events
    private void ProgressBuildOnTurnChanged(int _)
    {
        turnsLeft--;

        if (turnsLeft <= 0)
            Game.World.CreateUnitAt(position, unitToBuild, team);

        GlobalEventManager.OnNextTurn.RemoveListener(ProgressBuildOnTurnChanged);
    }
    #endregion
}
