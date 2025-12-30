using System.Collections;
using System.Collections.Generic;

public class Transport : Unit
{
    private List<Unit> units = new();
    private int maxCapacity = 3;

    protected override IEnumerator AnimateMove(List<VectorHex> path)
    {
        yield return MoveByPath(path);
    }
    protected override List<VectorHex> FindPath(VectorHex _, VectorHex to)
    {
        return new() { to };
    }
}
