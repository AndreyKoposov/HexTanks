using UnityEngine;

public struct VectorHex
{
    public static readonly VectorHex UNSIGNED = new (-Vector3Int.one);

    private Vector3Int coords;
    public readonly int X => coords.x;
    public readonly int Y  => coords.y;
    public readonly bool Unsigned => coords == -Vector3Int.one;

    public VectorHex(Vector3Int pos)
    {
        coords = pos;
    }

    #region Overrides
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
