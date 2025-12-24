using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    private readonly Dictionary<Vector3Int, Transform> map = new();

    private void Start()
    {
        foreach (var cell in tilemap.GetComponentsInChildren<Transform>())
        {
            var cellPos = tilemap.WorldToCell(cell.position);
            map[cellPos] = cell;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 20f))
            {
                var cellPos = tilemap.WorldToCell(hit.point);
                var cell = map[cellPos];
                cell.transform.position += Vector3.up;
            }
        }
    }
}
