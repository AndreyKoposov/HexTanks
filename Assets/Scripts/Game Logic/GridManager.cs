using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap obstacles;

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
            cell.Setup((VectorHex)tilemap.WorldToCell(cell.transform.position));
            map[cell.Position] = cell;
        }

        foreach (var obstacle in obstacles.GetComponentsInChildren<Obstacle>())
        {
            var pos = (VectorHex)tilemap.WorldToCell(obstacle.transform.position);
            map[pos].Obstacle = obstacle;
        }
    }

    public void MoveUnitFromTo(VectorHex from, VectorHex to)
    {
        if (from == to) return;

        Unit unit = map[from].UnsetUnit();
        map[to].SetUnit(unit);
    }
    public void AttackUnitAt(VectorHex attacking, VectorHex attacked)
    {
        var attackingUnit = map[attacking].Unit;
        var attackedUnit = map[attacked].Unit;

        attackingUnit.AttackUnit(attackedUnit);
    }
    public List<VectorHex> GetValidMovesForUnit(VectorHex position)
    {
        List<VectorHex> result = new();

        Unit unit = map[position].Unit;

        if (Game.CurrentPlayer != unit.Team)
            return result;
        if (!unit.CanMove)
            return result;

        HashSet<VectorHex> positions = GetRing(new() { position }, 
                                               unit.MovePoints, 
                                               unit.CanMoveThroughTile);

        foreach (VectorHex pos in positions)
            if (!map[pos].HasUnit)
                result.Add(pos);

        return result;
    }

    public List<VectorHex> GetValidAttacksForUnit(VectorHex position)
    {
        List<VectorHex> result = new();

        Unit unit = map[position].Unit;

        if (Game.CurrentPlayer != unit.Team)
            return result;
        if (!unit.CanAttack)
            return result;

        HashSet<VectorHex> positions = GetRing(new() { position }, 
                                               unit.Info.MaxAttackDistance, 
                                               (_) => true);

        foreach (VectorHex pos in positions)
            if (unit.CanAttackTile(map[pos]))
                result.Add(pos);

        return result;
    }

    private HashSet<VectorHex> GetRing(HashSet<VectorHex> prevRing, int iter, Predicate<HexTile> filter)
    {
        if (iter <= 0)
            return prevRing;

        HashSet<VectorHex> res = new();
        foreach (var pos in prevRing)
            res.UnionWith(pos.Neighbours.Where(p => map.Keys.Contains(p) && filter(map[p])));

        prevRing.UnionWith(GetRing(res, iter - 1, filter));

        return prevRing;
    }
}
