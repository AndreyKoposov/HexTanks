using System.Collections;

public class Heavy : Unit
{
    public override void AttackUnit(Unit attacked)
    {
        StartCoroutine(RotateAndAttack(attacked));
    }

    private IEnumerator RotateAndAttack(Unit attacked)
    {
        yield return RotateTo(Game.Grid[attacked.Position].transform.position);

        base.AttackUnit(attacked);
    }
}
