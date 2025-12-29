using System.Collections;

public class Heavy : Unit
{
    protected override IEnumerator AnimateAttack(Unit attacked)
    {
        yield return RotateTo(transform, Game.Grid[attacked.Position].transform.position);
    }
}
