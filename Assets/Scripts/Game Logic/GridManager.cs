using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public void CreateUnitAt(VectorHex position, Team team)
    {
        HexTile tile = map[position];

        if (tile.HasUnit) return;
        if (tile.isObstacle) return;

        var unit = fabric.CreateUnit(tile, team);

        tile.SetUnit(unit);
    }

    public void DestroyUnitAt(VectorHex position)
    {
        HexTile tile = map[position];

        if (tile.HasUnit)
        {
            Destroy(tile.unit.gameObject);
            tile.UnsetUnit();
        }
    }

    public void MoveUnitFromTo(VectorHex from, VectorHex to)
    {
        var unit = map[from].unit;

        map[from].UnsetUnit();
        map[to].SetUnit(unit);

        unit.MoveTo(map[to]);
    }

    public List<VectorHex> GetValidMovesForUnit(VectorHex position)
    {
        Unit unit = map[position].unit;

        List<VectorHex> result = new();
        HashSet <VectorHex> origin = new() { position };
        HashSet<VectorHex> positions = GetRing(origin, unit.info.MovementDistance);

        foreach (VectorHex pos in positions)
            if (map.Keys.Contains(pos) && !map[pos].isObstacle)
                result.Add(pos);

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
