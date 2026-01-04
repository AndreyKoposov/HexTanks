using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    private UnitType unitToBuild;
    private int turnsLeft;

    public UnitType UnitToBuild => unitToBuild;
    public int TurnsLeft => turnsLeft;

    public void StartBuildUnit(UnitType type)
    {
        unitToBuild = type;
        turnsLeft = 2;

        PlayerData player = state == Team.Player ? Game.Player : Game.Enemy;
        var targetInfo = Game.Fabric.GetInfoByType(type);

        player.plasm -= targetInfo.Plasm;
        player.titan -= targetInfo.Titan;
        player.chips -= targetInfo.Chips;
        Game.UI.UpdatePlayerPanel(player);

        GlobalEventManager.EndTurn.AddListener(ProgressBuildOnTurnChanged);
    }

    #region Events
    private void ProgressBuildOnTurnChanged(Team nextPlayer)
    {
        if (state == nextPlayer) return;

        PlayerData player = state == Team.Player ? Game.Player : Game.Enemy;
        if (player.unitsHas >= player.unitsMax)
        {
            GlobalEventManager.EndTurn.RemoveListener(ProgressBuildOnTurnChanged);
            var targetInfo = Game.Fabric.GetInfoByType(unitToBuild);

            player.plasm += targetInfo.Plasm;
            player.titan += targetInfo.Titan;
            player.chips += targetInfo.Chips;
            Game.UI.UpdatePlayerPanel(player);

            return;
        }

        turnsLeft--;

        if (turnsLeft <= 0)
        {
            Game.Fabric.CreateUnitAt(position, unitToBuild, state);

            GlobalEventManager.EndTurn.RemoveListener(ProgressBuildOnTurnChanged);
        }
    }
    #endregion
}
