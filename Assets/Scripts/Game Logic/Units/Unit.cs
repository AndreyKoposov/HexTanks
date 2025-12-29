using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected static Vector3 OffsetOverTile = Vector3.zero - Vector3.forward * 0.09f;

    public UnitInfo info;

    protected int hp;
    protected Team team;
    protected VectorHex position;

    protected int movePoints;
    protected int attackPoints;

    public bool Dead => hp <= 0;
    public bool CanMove => movePoints > 0 && attackPoints == info.AttackPoints;
    public bool CanAttack => attackPoints > 0 && movePoints == info.MovementDistance;

    public Team Team => team;
    public VectorHex Position => position;
    public int MovePoints => movePoints;

    private void Awake()
    {
        RegisterOnEvents();
    }

    public void Setup(Team team)
    {
        SetTeam(team);

        hp = info.Hp;
        movePoints = info.MovementDistance;
        attackPoints = info.AttackPoints;

        transform.rotation = Quaternion.identity;
    }

    #region Actions
    public virtual void MoveTo(HexTile to, bool spawn)
    {
        if (spawn)
            SetPosition(to);
        else
        {
            List<VectorHex> path = A_Star.FindShortestPath(position, to.Position);
            StartCoroutine(MoveByPath(path));

            movePoints -= path.Count;
        }
    }
    public virtual void AttackUnit(Unit attacked)
    {
        attacked.DealDamage(info.Damage);

        attackPoints--;
    }
    public void DealDamage(int damage)
    {
        hp -= damage;
    }
    #endregion

    #region Operations
    protected void SetPosition(HexTile to)
    {
        position = to.Position;

        gameObject.transform.parent = to.gameObject.transform;
        transform.localPosition = OffsetOverTile;
    }
    protected void SetTeam(Team team)
    {
        this.team = team;

        if (team == Team.Player)
            GetComponent<MeshRenderer>().material = Game.Art.PlayerMat;
        if (team == Team.Enemy)
            GetComponent<MeshRenderer>().material = Game.Art.EnemyMat;
    }
    protected IEnumerator MoveByPath(List<VectorHex> path)
    {
        foreach (VectorHex p in path)
        {
            yield return Rotate(p);
            yield return MoveByLine(Game.Grid[p].transform.position);
            SetPosition(Game.Grid[p]);
        }
    }
    protected IEnumerator Rotate(VectorHex p)
    {
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

        var delta = targetAngle - transform.rotation.eulerAngles.y;

        if (Mathf.Abs(delta) > Mathf.Abs(delta + 360))
            delta += 360;

        if (Mathf.Abs(delta) > 0.01f)
            for (int j = 0; j < frames; j++)
            {
                transform.Rotate(Vector3.up, delta / frames);
                yield return new WaitForSeconds(0.2f / frames);
            }
    }
    protected IEnumerator MoveByLine(Vector3 point)
    {
        int frames = 20;
        float delta = (transform.position - point).magnitude;
        for (int j = 0; j < frames; j++)
        {
            transform.Translate(delta / frames * Vector3.left, Space.Self);
            yield return new WaitForSeconds(0.3f / frames);
        }
    }
    #endregion

    #region Events
    protected void RegisterOnEvents()
    {
        GlobalEventManager.TurnChanged.AddListener(ResetOnTurnChanged);
    }
    protected void ResetOnTurnChanged(int _)
    {
        movePoints = info.MovementDistance;
        attackPoints = info.AttackPoints;
    }
    #endregion
}
