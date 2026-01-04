public class PlayerData
{
    public Team team;

    public int unitsHas;
    public int unitsMax;

    public int plasm;
    public int titan;
    public int chips;

    public int plasmProd;
    public int titanProd;
    public int chipsProd;

    public bool LimitReached => unitsHas >= unitsMax;

    public PlayerData(Team team)
    {
        this.team = team;

        RegisterOnEvents();
    }

    public bool CanBuildUnit(UnitType type)
    {
        var targetInfo = Game.Fabric.GetInfoByType(type);

        if (targetInfo.Plasm > plasm) return false;
        if (targetInfo.Titan > titan) return false;
        if (targetInfo.Chips > chips) return false;

        return true;
    }

    public void AddBuilding(BuildingInfo info)
    {
        unitsMax += info.UnitsContain;
        plasmProd += info.ProducePlasm;
        titanProd += info.ProduceTitan;
        chipsProd += info.ProduceChips;

        Game.UI.UpdatePlayerPanel(this);
    }
    public void RemoveBuilding(BuildingInfo info)
    {
        unitsMax -= info.UnitsContain;
        plasmProd -= info.ProducePlasm;
        titanProd -= info.ProduceTitan;
        chipsProd -= info.ProduceChips;

        Game.UI.UpdatePlayerPanel(this);
    }
    public void AddResourcesForBuilding(BuildingInfo info)
    {
        plasm += info.ProducePlasm;
        titan += info.ProduceTitan;
        chips += info.ProduceChips;

        Game.UI.UpdatePlayerPanel(this);
    }
    public void AddResourcesForUnit(UnitType type)
    {
        var targetInfo = Game.Fabric.GetInfoByType(type);

        plasm += targetInfo.Plasm;
        titan += targetInfo.Titan;
        chips += targetInfo.Chips;

        Game.UI.UpdatePlayerPanel(this);
    }
    public void SubstactResourcesForUnit(UnitType type)
    {
        var targetInfo = Game.Fabric.GetInfoByType(type);

        plasm -= targetInfo.Plasm;
        titan -= targetInfo.Titan;
        chips -= targetInfo.Chips;

        Game.UI.UpdatePlayerPanel(this);
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.UnitDied.AddListener(OnUnitDied);
        GlobalEventManager.UnitCreated.AddListener(OnUnitCreated);
    }
    private void OnUnitCreated(VectorHex _, Team team)
    {
        if (team == this.team)
        {
            unitsHas++;
            Game.UI.UpdatePlayerPanel(this);
        }
    }
    private void OnUnitDied(VectorHex _, Team team)
    {
        if (team == this.team)
        {
            unitsHas--;
            Game.UI.UpdatePlayerPanel(this);
        }
    }
    #endregion
}
