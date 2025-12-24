using System.Collections.Generic;
using System.Drawing;
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

    public void CreateUnit(Vector3Int position, Team team)
    {
        var unit = Instantiate(prefabs[0], map[position].transform).GetComponent<Tank>();

        unit.Setup(team, position);

        units[position] = unit;
    }

    public void DestroyUnit(Vector3Int position)
    {
        Destroy(units[position].gameObject);
    }
}
