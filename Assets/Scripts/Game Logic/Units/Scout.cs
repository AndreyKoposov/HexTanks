using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : Unit
{
    protected override IEnumerator AnimateAttack(Unit attacked)
    {
        yield return RotateTo(transform, Game.Grid[attacked.Position].transform.position);
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
