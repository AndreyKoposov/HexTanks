using System;

public class Node : IComparable<Node>
{
    public Node parent;
    public VectorHex position;
    public float G;
    public float H;
    public float F => G + H;

    public int CompareTo(Node other)
    {
        return F.CompareTo(other.F);
    }
}
