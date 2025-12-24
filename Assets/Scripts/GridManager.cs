using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    // 0.866025
    [SerializeField] private Tilemap tilemap;

    private readonly Dictionary<Vector3Int, HexTile> map = new();
    private Vector3Int prevHighlight;

    public bool InsideMap { get; private set; }

    private void Start()
    {
        foreach (var cell in tilemap.GetComponentsInChildren<HexTile>())
        {
            cell.position = tilemap.WorldToCell(cell.transform.position);
            map[cell.position] = cell;
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 20f))
        {
            var tile = hit.collider.gameObject.GetComponent<HexTile>();
            var curHighlight = tile.position;

            if (prevHighlight != curHighlight)
            {
                map[prevHighlight].gameObject.layer = LayerMask.NameToLayer("Grid");
                prevHighlight = curHighlight;
                tile.gameObject.layer = LayerMask.NameToLayer("Highlight");
            }
            else
            {
                prevHighlight = curHighlight;
            }
        }
        else
        {
            map[prevHighlight].gameObject.layer = LayerMask.NameToLayer("Grid");
        }
    }
}
