using System.Collections.Generic;
using UnityEngine;

public class UnitFabric : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new();

    private void Awake()
    {
        RegisterOnEvents();
    }

    public void CreateUnitAt(VectorHex position, UnitType type, Team team)
    {
        Unit unit = Instantiate(GetPrefab(type)).GetComponent<Unit>();
        HexTile tile = Game.Grid[position];

        unit.Setup(team);

        Game.Grid[position].SetUnit(unit);
        unit.SpawnAt(tile);

        GlobalEventManager.UnitCreated.Invoke(unit.Position, unit.Team);
    }

    public void DestroyUnitAt(VectorHex position, Team _)
    {
        Unit unit = Game.Grid[position].UnsetUnit();
        Destroy(unit.gameObject);
    }

    private GameObject GetPrefab(UnitType type)
    {
        return prefabs.Find(p => p.GetComponent<Unit>().Info.Type == type);
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.UnitDied.AddListener(DestroyUnitAt);
    }
    #endregion
}
