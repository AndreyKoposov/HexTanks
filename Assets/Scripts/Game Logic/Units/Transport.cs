using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Transport : Unit
{
    private static readonly int MaxCapacity = 3;

    private readonly List<Unit> units = new();

    public int Count => units.Count;
    public bool CanBoard => units.Count < MaxCapacity;
    public Unit this[int i]
    {
        get => units[i];
    }

    public void SetUnit(Unit unit)
    {
        units.Add(unit);
    }
    public Unit UnsetUnitOn(int index)
    {
        var unit = units[index];
        units.RemoveAt(index);

        return unit;
    }

    protected override IEnumerator AnimateMove(List<VectorHex> path, int scaleOption=-1)
    {
        yield return MoveByPath(path, scaleOption);

        units.ForEach(unit => unit.SetGlobalPositionTo(Game.Grid[position]));
    }
    protected override List<VectorHex> FindPath(VectorHex _, VectorHex to)
    {
        return new() { to };
    }
}
