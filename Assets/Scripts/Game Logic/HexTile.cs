using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HexTile : MonoBehaviour
{
    public UnityEvent<Team> OnUnitSet { get; set; } = new();
    public UnityEvent<Team> OnUnitUnset { get; set; } = new();

    [SerializeField] private SpriteRenderer[] perimeter = new SpriteRenderer[6];
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
    public Factory Building
    {
        get => obstacle as Factory;
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
        get => obstacle != null && obstacle is Factory;
    }
    public bool Protected
    {
        get => protectedBy != VectorHex.UNSIGNED;
    }

    public void Setup(VectorHex position)
    {
        this.position = position;
    }
    public void SetUnit(Unit unit)
    {
        this.unit = unit;

        OnUnitSet.Invoke(unit.Team);
    }
    public Unit UnsetUnit()
    {
        var unsetted = unit;
        unit = null;

        OnUnitUnset.Invoke(unsetted.Team);

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

    public void SetTerritory(Team team, List<HexDirections> sides)
    {
        var color = team switch
        {
            Team.Player => Game.Art.PlayerColor,
            Team.Enemy => Game.Art.EnemyColor,
            Team.Neutral => Game.Art.NeutralColor,
            Team.Blocked => Game.Art.BlockedColor,
            _ => Game.Art.NeutralColor,
        };

        foreach (var sprite in perimeter)
            sprite.gameObject.SetActive(false);

        for (int i = 0; i < sides.Count; i++)
        {
            int direction = (int)sides[i];

            perimeter[direction].gameObject.SetActive(true);
            perimeter[direction].color = color;
        }
    }
}
