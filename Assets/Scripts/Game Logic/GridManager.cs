using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private GameObject[] prefabs;

    private readonly Dictionary<Vector3Int, HexTile> map = new();
    private readonly Dictionary<Vector3Int, Tank> units = new();

    private void Start()
    {
        InitMap();
    }

    private void InitMap()
    {
        foreach (var cell in tilemap.GetComponentsInChildren<HexTile>())
        {
            cell.position = tilemap.WorldToCell(cell.transform.position);
            map[cell.position] = cell;
        }
    }

    public void CreateUnitAt(Vector3Int position, Team team)
    {
        HexTile tile = map[position];

        if (tile.HasUnit) return;
        if (tile.isObstacle) return;

        var unit = Instantiate(prefabs[0], 
                               Vector3.zero, 
                               Quaternion.identity, 
                               map[position].transform).GetComponent<Tank>();

        unit.Setup(team, position);

        map[position].tank = unit;
        units[position] = unit;
    }

    public void DestroyUnitAt(Vector3Int position)
    {
        if(map[position].HasUnit)
            Destroy(units[position].gameObject);
    }

    public List<HexTile> GetValidMovesForUnit(Vector3Int position)
    {
        Tank unit = units[position];

        List<HexTile> result = new();
        HashSet <Vector3Int> origin = new() { position };
        HashSet<Vector3Int> positions = GetRing(origin, unit.movementDistance);

        foreach (Vector3Int pos in positions)
            if (map.Keys.Contains(pos) && !map[pos].isObstacle)
                result.Add(map[pos]);

        return result;
    }

    private HashSet<Vector3Int> GetRing(HashSet<Vector3Int> prevRing, int iter)
    {
        if (iter <= 0)
            return prevRing;

        HashSet<Vector3Int> res = new();
        foreach (var pos in prevRing)
            res.UnionWith(GetNeighbours(pos));

        prevRing.UnionWith(GetRing(res, iter - 1));

        return prevRing;
    }

    private HashSet<Vector3Int> GetNeighbours(Vector3Int position)
    {
        int even = Mathf.Abs(position.y) % 2;

        return new()
        {
            position + new Vector3Int(1, 0,  0),   // Вправо
            position + new Vector3Int(0 + even, 1, 0),   // Вправо-вверх
            position + new Vector3Int(-1 + even, 1, 0),   // Влево-вверх
            position + new Vector3Int(-1, 0, 0),   // Влево
            position + new Vector3Int(-1 + even, -1, 0),   // Влево-вниз
            position + new Vector3Int(0 + even, -1, 0)    // Вправо-вниз
        };
    }
}
