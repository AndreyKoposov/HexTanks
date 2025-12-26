using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private UnitFabric fabric;

    private readonly Dictionary<VectorHex, HexTile> map = new();

    public HexTile this[VectorHex vh]
    {
        get => map[vh];
    }

    private void Start()
    {
        InitMap();
    }
    private void InitMap()
    {
        foreach (var cell in tilemap.GetComponentsInChildren<HexTile>())
        {
            cell.position = (VectorHex)tilemap.WorldToCell(cell.transform.position);
            map[cell.position] = cell;
        }
    }

    public void CreateUnitAt(VectorHex position, UnitType type, Team team)
    {
        HexTile tile = map[position];

        if (tile.HasUnit) return;
        if (tile.isObstacle) return;

        fabric.CreateUnitAt(tile, type, team);
    }

    public void DestroyUnitAt(VectorHex position)
    {
        HexTile tile = map[position];

        if (!tile.HasUnit) return;

        fabric.DestroyUnitAt(tile);
    }

    public void MoveUnitFromTo(VectorHex from, VectorHex to)
    {
        var unit = map[from].unit;

        map[from].UnsetUnit();
        map[to].SetUnit(unit);

        unit.MoveTo(map[to]);
    }
    public void AttackUnitAt(VectorHex attacking, VectorHex attacked)
    {
        var attackingUnit = map[attacking].unit;
        var attackedUnit = map[attacked].unit;

        attackedUnit.DealDamage(attackingUnit.info.Damage);

        if (attackedUnit.Dead)
            fabric.DestroyUnitAt(map[attacked]);
    }

    public List<VectorHex> GetValidMovesForUnit(VectorHex position)
    {
        Unit unit = map[position].unit;

        List<VectorHex> result = new();
        HashSet <VectorHex> origin = new() { position };
        HashSet<VectorHex> positions = GetRing(origin, unit.info.MovementDistance);

        foreach (VectorHex pos in positions)
            if (map.Keys.Contains(pos) && !map[pos].HasUnit)
                result.Add(pos);

        return result;
    }

    public List<VectorHex> GetValidAttacksForUnit(VectorHex position)
    {
        Unit unit = map[position].unit;

        List<VectorHex> result = GetNeighbours(position).Where(p => map[p].HasUnit).ToList();

        return result;
    }

    private HashSet<VectorHex> GetRing(HashSet<VectorHex> prevRing, int iter)
    {
        if (iter <= 0)
            return prevRing;

        HashSet<VectorHex> res = new();
        foreach (var pos in prevRing)
            res.UnionWith(GetNeighbours(pos).Where(p => !map[p].isObstacle));

        prevRing.UnionWith(GetRing(res, iter - 1));

        return prevRing;
    }

    private HashSet<VectorHex> GetNeighbours(VectorHex position)
    {
        return new()
        {
            position + position.Right,
            position + position.Left,
            position + position.RightBottom,
            position + position.RightTop,
            position + position.LeftBottom,
            position + position.LeftTop,
        };
    }
}
