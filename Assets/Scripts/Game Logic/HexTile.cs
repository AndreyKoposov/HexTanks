using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer select;

    public VectorHex position;
    public Unit unit;
    public Obstacle obstacle;
    public bool isWater;

    public bool IsObstacle => isWater || obstacle != null;

    public bool HasUnit
    {
        get { return unit != null; }
    }
    public bool HasBuilding
    {
        get => obstacle != null && obstacle is Building;
    }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
    }

    public void SetObstacle(Obstacle obs)
    {
        obstacle = obs;
        
        if (obstacle is Building building)
        {
            building.position = position;
        }
    }

    public void UnsetUnit()
    {
        unit = null;
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
