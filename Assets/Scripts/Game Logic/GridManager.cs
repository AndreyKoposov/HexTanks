using log4net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

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

        if (map[to].Unit is Transport)
        {
            var transport = map[to].Unit as Transport;

            transport.SetUnit(unit);
            unit.BoardTo(transport);
        }
        else
        {
            map[to].SetUnit(unit);
            unit.MoveTo(map[to]);
        }
    }
    public void UnboardUnitFromTo(VectorHex from, VectorHex to, int transportIndex)
    {
        if (from == to) return;
        var transport = map[from].Unit as Transport;

        var unit = transport.UnsetUnitOn(transportIndex);
        map[to].SetUnit(unit);
        unit.UnboardFrom(transport, to);
    }
    public void AttackUnitAt(VectorHex attacking, VectorHex attacked)
    {
        var attackingUnit = map[attacking].Unit;
        var attackedUnit = map[attacked].Unit;

        attackingUnit.AttackUnit(attackedUnit);
    }
    public List<VectorHex> GetValidMoves(VectorHex position)
    {
        return GetValidMovesForUnit(map[position].Unit, position);
    }

    public List<VectorHex> GetValidMoves(Transport transport, int index)
    {
        Unit toUnboard = transport[index];
        HexTile origin = map[transport.Position];

        if (toUnboard.CanMoveThroughTile(origin))
            return GetValidMovesForUnit(toUnboard, transport.Position);
        else
            return new();
    }
    public List<VectorHex> GetValidMovesForUnit(Unit unit, VectorHex origin)
    {
        List<VectorHex> result = new();

        if (Game.CurrentTeam != unit.Team)
            return result;
        if (!unit.CanMove)
            return result;

        HashSet<VectorHex> positions = GetRing(new() { origin },
                                               unit.MovePoints,
                                               unit.CanMoveThroughTile);

        foreach (VectorHex pos in positions)
            if (unit.CanStayOnTile(map[pos]))
                result.Add(pos);

        return result;
    }

    public List<VectorHex> GetValidAttacksForUnit(VectorHex position)
    {
        List<VectorHex> result = new();

        Unit unit = map[position].Unit;

        if (Game.CurrentTeam != unit.Team)
            return result;
        if (!unit.CanAttack)
            return result;

        HashSet<VectorHex> positions = GetRing(new() { position }, 
                                               unit.Info.MaxAttackDistance);

        foreach (VectorHex pos in positions)
            if (unit.CanAttackTile(map[pos]))
                result.Add(pos);

        return result;
    }

    public HashSet<VectorHex> GetRing(HashSet<VectorHex> prevRing, int iter, Predicate<HexTile> rule=null)
    {
        if (iter <= 0)
            return prevRing;

        rule ??= _ => true;

        HashSet<VectorHex> res = new();
        foreach (var pos in prevRing)
            res.UnionWith(pos.Neighbours.Where(p => map.Keys.Contains(p) && rule(map[p])));

        prevRing.UnionWith(GetRing(res, iter - 1, rule));

        return prevRing;
    }
}
