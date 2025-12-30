using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Transport : Unit
{
    private static readonly int MaxCapacity = 3;

    public readonly List<Unit> units = new();

    public bool CanBoard => units.Count < MaxCapacity;

    public void SetUnit(Unit unit)
    {
        units.Add(unit);
        unit.BoardTo(this);
    }
    public void UnsetUnitOn(int index, VectorHex on)
    {
        var unit = units[index];
        units.RemoveAt(index);
        unit.UnboardFrom(this, on);
    }

    protected override IEnumerator AnimateMove(List<VectorHex> path)
    {
        yield return MoveByPath(path);
    }
    protected override List<VectorHex> FindPath(VectorHex _, VectorHex to)
    {
        return new() { to };
    }
}
