using UnityEngine;

public class HexTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer select;
    [SerializeField] private bool isWater;

    private VectorHex position;
    private Unit unit;
    private Obstacle obstacle;
    private VectorHex protectedBy = VectorHex.UNSIGNED;

    public Obstacle Obstacle
    {
        get => obstacle;
        set 
        {
            obstacle = value;

            if (obstacle is Building building)
                building.position = position;
        }
    }
    public Building Building
    {
        get => obstacle as Building;
    }
    public VectorHex Position => position;
    public Unit Unit => unit;
    public VectorHex ProtectedBy => protectedBy;

    public bool IsObstacle => isWater || obstacle != null;
    public bool HasUnit
    {
        get { return unit != null; }
    }
    public bool HasBuilding
    {
        get => obstacle != null && obstacle is Building;
    }

    public void Setup(VectorHex position)
    {
        this.position = position;
    }
    public void SetUnit(Unit unit, bool spawn = false)
    {
        this.unit = unit;

        if (spawn)
            this.unit.SpawnAt(this);
        else
            this.unit.MoveTo(this);
    }
    public Unit UnsetUnit()
    {
        var unsetted = unit;
        unit = null;

        return unsetted;
    }
    public void SetProtection(Unit unit)
    {
        protectedBy = unit.Position;
    }
    public void UnsetProtection()
    {
        protectedBy = VectorHex.UNSIGNED;
    }
    public void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
    public void ApplySelect(SelectType type)
    {
        switch (type)
        {
            case SelectType.None:
                select.gameObject.SetActive(false);
                break;
            case SelectType.Default:
                select.gameObject.SetActive(true);
                select.color = Game.Art.SelectColor;
                break;
            case SelectType.Attack:
                select.gameObject.SetActive(true);
                select.color = Game.Art.AttackColor;
                break;
        }
    }
}
