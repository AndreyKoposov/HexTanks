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

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.PlayerGotBuilding.AddListener(OnBuildingGot);
        GlobalEventManager.PlayerLoseBuilding.AddListener(OnBuildingLose);
        GlobalEventManager.UnitDied.AddListener(OnUnitDied);
        GlobalEventManager.UnitCreated.AddListener(OnUnitCreated);
    }
    private void OnBuildingGot(Team team, BuildingInfo info)
    {
        if (team == this.team)
        {
            unitsMax += info.UnitsContain;
            plasmProd += info.ProducePlasm;
            titanProd += info.ProduceTitan;
            chipsProd += info.ProduceChips;

            Game.UI.UpdatePlayerPanel(this);
        }
    }
    private void OnBuildingLose(Team team, BuildingInfo info)
    {
        if (team == this.team)
        {
            unitsMax -= info.UnitsContain;
            plasmProd -= info.ProducePlasm;
            titanProd -= info.ProduceTitan;
            chipsProd -= info.ProduceChips;

            Game.UI.UpdatePlayerPanel(this);
        }
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
