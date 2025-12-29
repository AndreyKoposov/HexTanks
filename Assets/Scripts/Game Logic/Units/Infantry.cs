using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Infantry : Unit
{
    [SerializeField] private GameObject tower;

    private void Awake()
    {
        RegisterOnEvents();
    }

    public override void AttackUnit(Unit attacked)
    {
        StartCoroutine(RotateTower(attacked));
    }

    protected IEnumerator RotateTower(Unit attacked)
    {
        int frames = 20;

        var tilePos = Game.Grid[attacked.Position].transform.position;

        Vector3 lookDirection = tower.transform.forward;
        Vector3 targetDirection = (new Vector3(tilePos.x, transform.position.y, tilePos.z) - transform.position).normalized;
        float targetAngle = Vector3.SignedAngle(lookDirection, targetDirection, Vector3.up);

        if (Mathf.Abs(targetAngle) > 0.01f)
            for (int j = 0; j < frames; j++)
            {
                tower.transform.Rotate(Vector3.up, targetAngle / frames);
                yield return new WaitForSeconds(0.2f / frames);
            }

        base.AttackUnit(attacked);
    }
}
