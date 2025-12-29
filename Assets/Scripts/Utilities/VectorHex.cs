using System.Collections.Generic;
using UnityEngine;

public struct VectorHex
{
    public static readonly VectorHex UNSIGNED = new (-Vector3Int.one);

    private Vector3Int coords;
    public readonly int X => coords.x;
    public readonly int Y  => coords.y;
    public readonly bool Unsigned => coords == -Vector3Int.one;
    public readonly HashSet<VectorHex> Neighbours
    {
        get => new()
        {
            this + Right,
            this + Left,
            this + RightBottom,
            this + RightTop,
            this + LeftBottom,
            this + LeftTop,
        };
    }

    #region Directoins
    private readonly int Even => Mathf.Abs(Y) % 2;
    public readonly VectorHex Right => new(1, 0);
    public readonly VectorHex Left => new(-1, 0);
    public readonly VectorHex RightTop => new(0 + Even, 1);
    public readonly VectorHex RightBottom => new(0 + Even, -1);
    public readonly VectorHex LeftTop => new(-1 + Even, 1);
    public readonly VectorHex LeftBottom => new(-1 + Even, -1);
    #endregion

    public VectorHex(Vector3Int pos)
    {
        coords = pos;
    }
    public VectorHex(int x, int y)
    {
        coords = new(x, y, 0);
    }

    #region Overrides
    public static implicit operator Vector3Int(VectorHex vh) => vh.coords;
    public static explicit operator VectorHex(Vector3Int v3) => new (v3);
    public static VectorHex operator +(VectorHex vh1, VectorHex vh2) => (VectorHex)(vh1.coords + vh2.coords);
    public static int operator -(VectorHex vh1, VectorHex vh2)
    {
        var delta = vh2.coords - vh1.coords;

        return Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
    }
    public static bool operator ==(VectorHex vh1, VectorHex vh2) => vh1.Equals(vh2);
    public static bool operator !=(VectorHex vh1, VectorHex vh2) => !vh1.Equals(vh2);
    public override readonly bool Equals(object obj)
    {
        if (obj is not VectorHex)
            return false;
        else
            return coords == ((VectorHex)obj).coords;
    }
    public override int GetHashCode()
    {
        return coords.GetHashCode();
    }
    public override string ToString()
    {
        return coords.ToString();
    }
    #endregion
}
