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

    public int onBoardCount = 0;

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

    public IEnumerator MoveUp()
    {
        yield return AnimateVerticalMove(1);
    }
    public IEnumerator MoveDown()
    {
        yield return AnimateVerticalMove(-1);
    }

    private IEnumerator AnimateVerticalMove(int direction)
    {
        if (onBoardCount == 0)
        {
            float delta = Math.Abs(info.OffsetOverTile - 0.1f);

            for (int i = 0; i < Frames; i++)
            {
                transform.Translate(direction * delta / Frames * Vector3.up, Space.Self);
                yield return new WaitForSeconds(info.MoveSpeed / Frames);
            }
        }
    }

    protected override IEnumerator AnimateMove(List<VectorHex> path, int scaleOption=-1)
    {
        foreach (var unit in units)
            if (unit != null)
                unit.SetGlobalPositionTo(Game.Grid[position]);

        yield return MoveByPath(path, scaleOption);
    }
    protected override List<VectorHex> FindPath(VectorHex _, VectorHex to)
    {
        return new() { to };
    }
}
