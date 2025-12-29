using System;
using System.Collections;
using UnityEngine;

public class Infantry : Unit
{
    [SerializeField] private GameObject tower;

    protected override IEnumerator AnimateAttack(Unit attacked)
    {
        yield return RotateTo(tower.transform, Game.Grid[attacked.Position].transform.position);
    }
}
