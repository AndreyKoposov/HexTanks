using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : Factory
{
    [SerializeField] private Team initTeam;

    protected override void UpdateState()
    {
        Team newState;

        if (playerCounter > 0 && enemyCounter > 0)
            newState = Team.Blocked;
        else
        if (playerCounter > 0 && enemyCounter == 0)
            newState = Team.Player;
        else
        if (playerCounter == 0 && enemyCounter > 0)
            newState = Team.Enemy;
        else
            newState = initTeam;

        SetMaterial(newState);

        if (newState != state)
        {
            if (state != Team.Neutral && state != Team.Blocked)
                GlobalEventManager.PlayerLoseBuilding.Invoke(state, info);
            if (newState != Team.Neutral && newState != Team.Blocked)
                GlobalEventManager.PlayerGotBuilding.Invoke(newState, info);
        }

        state = newState;
    }
}
