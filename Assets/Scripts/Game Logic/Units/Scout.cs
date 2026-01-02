using System.Collections;
using System.Collections.Generic;

public class Scout : Unit
{
    protected override IEnumerator AnimateAttack(Unit attacked)
    {
        yield return RotateTo(transform, Game.Grid[attacked.Position].transform.position);
    }
    protected override IEnumerator AnimateMove(List<VectorHex> path, int scaleOption=-1)
    {
        yield return MoveByPath(path, scaleOption);
    }
    protected override List<VectorHex> FindPath(VectorHex _, VectorHex to)
    {
        return new() { to };
    }
}
