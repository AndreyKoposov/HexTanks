using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    private readonly Dictionary<Vector3Int, HexTile> map = new();

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
}
