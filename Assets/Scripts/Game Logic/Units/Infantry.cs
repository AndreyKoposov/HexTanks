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
        var p = attacked.Position;
        int frames = 20;
        float targetAngle = 0;

        if (p == position + position.Left)
            targetAngle = 0;
        if (p == position + position.LeftBottom)
            targetAngle = -60;
        if (p == position + position.RightBottom)
            targetAngle = -120;
        if (p == position + position.Right)
            targetAngle = -180;
        if (p == position + position.RightTop)
            targetAngle = -240;
        if (p == position + position.LeftTop)
            targetAngle = -300;

        var delta = targetAngle - tower.transform.rotation.eulerAngles.y;

        if (Mathf.Abs(delta) > Mathf.Abs(delta + 360))
            delta += 360;

        if (Mathf.Abs(delta) > 0.01f)
            for (int j = 0; j < frames; j++)
            {
                tower.transform.Rotate(Vector3.up, delta / frames);
                yield return new WaitForSeconds(0.2f / frames);
            }

        base.AttackUnit(attacked);
    }
}
