using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitInfo info;

    private int hp;
    private Team team;
    private VectorHex position;
    private bool canMove;

    public bool Dead => hp <= 0;

    private void Awake()
    {
        RegisterOnEvents();
    }

    public void Setup(Team Team)
    {
        team = Team;
        hp = info.Hp;
        canMove = true;
        transform.rotation = Quaternion.identity;
    }

    public void MoveTo(HexTile to, bool force = false)
    {
        position = to.position;

        gameObject.transform.parent = to.gameObject.transform;
        transform.localPosition = Vector3.zero - Vector3.forward * 0.09f;

        canMove = false;
    }

    public void DealDamage(int damage)
    {
        hp -= damage;
    }

    #region Events
    private void RegisterOnEvents()
    {
        GlobalEventManager.OnNextTurn.AddListener(ResetOnTurnChanged);
    }
    private void ResetOnTurnChanged(int _)
    {
        canMove = true;
    }
    #endregion
}
