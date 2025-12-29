using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : Unit
{
    public override void AttackUnit(Unit attacked)
    {
        StartCoroutine(RotateAndAttack(attacked));
    }
    public override void MoveTo(HexTile to, bool spawn)
    {
        if (spawn)
            SetGlobalPositionTo(to);
        else
        {
            List<VectorHex> path = new() { to.Position };
            StartCoroutine(MoveByPath(path));

            movePoints -= path.Count;
        }

        position = to.Position;
    }

    protected override void SetGlobalPositionTo(HexTile to)
    {
        base.SetGlobalPositionTo(to);
        transform.position += 10 * OffsetOverTile;
    }

    private IEnumerator RotateAndAttack(Unit attacked)
    {
        yield return RotateTo(Game.Grid[attacked.Position].transform.position);

        base.AttackUnit(attacked);
    }
}
