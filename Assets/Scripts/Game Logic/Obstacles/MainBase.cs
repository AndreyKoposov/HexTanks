using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : Factory
{
    [SerializeField] private Team initTeam;

    protected override void UpdateState()
    {
        base.UpdateState();

        if (enemyCounter == 0 && playerCounter == 0)
            state = initTeam;

        SetState(state);
    }
}
