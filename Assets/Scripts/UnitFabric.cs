using System.Collections.Generic;
using UnityEngine;

public class UnitFabric : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new();

    public void CreateUnitAt(HexTile tile, UnitType type, Team team)
    {
        Unit unit = Instantiate(GetPrefab(type)).GetComponent<Unit>();

        unit.Setup(team);
        tile.SetUnit(unit);

        GlobalEventManager.OnUnitCreated.Invoke(tile.position);
    }

    public void DestroyUnitAt(HexTile tile)
    {
        Unit unit = tile.unit;

        tile.UnsetUnit();
        Destroy(unit.gameObject);

        GlobalEventManager.OnUnitDestroyed.Invoke(tile.position);
    }

    private GameObject GetPrefab(UnitType type)
    {
        return prefabs.Find(p => p.GetComponent<Unit>().info.Type == type);
    }
}
