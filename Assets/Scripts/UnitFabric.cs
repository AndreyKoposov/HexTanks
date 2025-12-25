using System.Collections.Generic;
using UnityEngine;

public class UnitFabric : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new();

    public Unit CreateUnit(HexTile tile, Team team)
    {
        Unit unit = Instantiate(prefabs[0]).GetComponent<Unit>();

        unit.MoveTo(tile);
        unit.Setup(team);

        return unit;
    }

}
