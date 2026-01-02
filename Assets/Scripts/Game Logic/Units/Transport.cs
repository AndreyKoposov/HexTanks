using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Transport : Unit
{
    private static readonly int MaxCapacity = 3;

    private readonly Unit[] units = new Unit[MaxCapacity];

    public int Count => units.Length;
    public bool CanBoard => units.Any(u => u == null);
    public Unit this[int i]
    {
        get => units[i];
    }

    public void SetUnit(Unit unit)
    {
        int index = Array.FindIndex(units, u => u == null);
        units[index] = unit;
    }
    public Unit UnsetUnitOn(int index)
    {
        var unit = units[index];
        units[index] = null;

        return unit;
    }

    protected override IEnumerator AnimateMove(List<VectorHex> path, int scaleOption=-1)
    {
        yield return MoveByPath(path, scaleOption);

        Array.ForEach(units, unit => unit.SetGlobalPositionTo(Game.Grid[position]));
    }
    protected override List<VectorHex> FindPath(VectorHex _, VectorHex to)
    {
        return new() { to };
    }
}
