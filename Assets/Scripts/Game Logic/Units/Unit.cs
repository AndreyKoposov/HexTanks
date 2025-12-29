using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected static Vector3 OffsetOverTile = Vector3.up * 0.09f;
    protected static int Frames = 25;
    protected static float MoveSpeed = 0.3f;
    protected static float RotationSpeed = 0.2f;

    public UnitInfo info;

    protected Team team;
    protected VectorHex position;

    protected int hp;
    protected int movePoints;
    protected int attackPoints;

    public bool Dead => hp <= 0;
    public bool CanMove => movePoints > 0 && attackPoints == info.AttackPoints;
    public bool CanAttack => attackPoints > 0 && movePoints == info.MovementDistance;

    public Team Team => team;
    public VectorHex Position => position;
    public int MovePoints => movePoints;
    public UnitInfo Info => info;

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
            SetGlobalPositionTo(to);
        else
        {
            List<VectorHex> path = A_Star.FindShortestPath(position, to.Position);
            StartCoroutine(MoveByPath(path));

            movePoints -= position - to.Position;
        }

        position = to.Position;
    }
    public virtual void AttackUnit(Unit attacked)
    {
        attacked.DealDamage(info.Damage);

        attackPoints--;
    }
    public virtual void DealDamage(int damage)
    {
        hp -= damage;
    }
    #endregion

    #region Operations
    protected virtual void SetGlobalPositionTo(HexTile to)
    {
        transform.position = to.gameObject.transform.position;
        transform.position += OffsetOverTile;
    }
    protected void SetTeam(Team team)
    {
        this.team = team;

        //if (team == Team.Player)
        //    GetComponent<MeshRenderer>().material = Game.Art.PlayerMat;
        //if (team == Team.Enemy)
        //    GetComponent<MeshRenderer>().material = Game.Art.EnemyMat;
    }
    protected IEnumerator MoveByPath(List<VectorHex> path)
    {
        foreach (VectorHex p in path)
        {
            Vector3 tilePos = Game.Grid[p].transform.position;

            yield return RotateTo(tilePos);
            yield return MoveByLineTo(tilePos);
        }
    }
    protected IEnumerator RotateTo(Vector3 point)
    {
        Vector3 lookDirection = transform.forward;
        Vector3 targetDirection = (new Vector3(point.x, transform.position.y, point.z) - transform.position).normalized;

        float targetAngle = Vector3.SignedAngle(lookDirection, targetDirection, Vector3.up);

        if (Mathf.Abs(targetAngle) > 0.01f)
            for (int i = 0; i < Frames; i++)
            {
                transform.Rotate(Vector3.up, targetAngle / Frames);
                yield return new WaitForSeconds(RotationSpeed / Frames);
            }
    }
    protected IEnumerator MoveByLineTo(Vector3 point)
    {
        float delta = (transform.position - point).magnitude;
        for (int i = 0; i < Frames; i++)
        {
            transform.Translate(delta / Frames * Vector3.forward, Space.Self);
            yield return new WaitForSeconds(MoveSpeed / Frames);
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
