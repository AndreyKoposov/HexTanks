using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected static int Frames = 25;

    public UnitInfo info;
    public List<MeshRenderer> colorParts = new();

    protected Team team;
    protected VectorHex position;

    protected int hp;
    protected int movePoints;
    protected int attackPoints;
    protected List<VectorHex> attackedUnits = new();

    public bool CanMove => movePoints > 0 && attackPoints == info.AttackPoints;
    public bool CanAttack => attackPoints > 0 && movePoints == info.MovementDistance;

    public Team Team => team;
    public VectorHex Position => position;
    public int MovePoints => movePoints;
    public UnitInfo Info => info;

    protected void Awake()
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
    public bool CanMoveThroughTile(HexTile tile)
    {
        if (tile.HasUnit && tile.Unit.Team != team)
            return false;

        if (!info.Flying && tile.IsObstacle)
            return false;
        
        return true;
    }
    public bool CanAttackTile(HexTile tile)
    {
        if (!tile.HasUnit || tile.Unit.Team == team || attackedUnits.Contains(tile.Position))
            return false;

        if (position - tile.Position <= info.MinAttackDistance)
            return false;

        return true;
    }

    #region Actions
    public void SpawnAt(HexTile tile)
    {
        position = tile.Position;
        SetGlobalPositionTo(tile);
    }
    public void MoveTo(HexTile to)
    {
        var path = FindPath(position, to.Position);

        void preAction()
        {
            movePoints -= position - to.Position;
            position = to.Position;
        }
        void postAction()
        {
            SetGlobalPositionTo(to);
        }

        StartCoroutine(Wrapper(
            preAction,
            () => AnimateMove(path),
            postAction
        ));
    }
    public void AttackUnit(Unit attacked)
    {
        void preAction()
        {
            attackPoints--;
            attackedUnits.Add(attacked.Position);
        }
        void postAction()
        {
            attacked.DealDamage(info.Damage);
        }

        StartCoroutine(Wrapper(
            preAction,
            () => AnimateAttack(attacked),
            postAction
        ));
    }
    public void DealDamage(int damage)
    {
        void preAction()
        {
            hp -= damage;
        }
        void postAction()
        {
            if (hp <= 0)
                GlobalEventManager.UnitDied.Invoke(position);
        }

        StartCoroutine(Wrapper(
            preAction,
            () => AnimateDamage(),
            postAction
        ));
    }
    #endregion

    #region Virtuals
    protected virtual IEnumerator AnimateMove(List<VectorHex> path)
    {
        yield return MoveByPath(path);
    }
    protected virtual IEnumerator AnimateAttack(Unit attacked)
    {
        yield return null;
    }
    protected virtual IEnumerator AnimateDamage()
    {
        yield return null;
    }
    protected virtual List<VectorHex> FindPath(VectorHex from, VectorHex to)
    {
        return A_Star.FindShortestPath(from, to);
    }
    #endregion

    #region Operations
    protected IEnumerator Wrapper(Action preAction, Func<IEnumerator> animation, Action postAction)
    {
        preAction();
        yield return animation();
        postAction();
    }
    protected void SetGlobalPositionTo(HexTile to)
    {
        transform.position = to.gameObject.transform.position;
        transform.position += Vector3.up * info.OffsetOverTile;
    }
    protected void SetTeam(Team team)
    {
        this.team = team;

        Material materialToSet;
        if (team == Team.Player)
            materialToSet = Game.Art.PlayerMat;
        else
            materialToSet = Game.Art.EnemyMat;

        foreach (var part in colorParts)
            part.material = materialToSet;
    }
    protected IEnumerator MoveByPath(List<VectorHex> path)
    {
        foreach (VectorHex p in path)
        {
            Vector3 tilePos = Game.Grid[p].transform.position;

            yield return RotateTo(transform, tilePos);
            yield return MoveByLineTo(tilePos);
        }
    }
    protected IEnumerator RotateTo(Transform part, Vector3 point)
    {
        Vector3 lookDirection = part.forward;
        Vector3 targetDirection = (new Vector3(point.x, part.position.y, point.z) - part.position).normalized;

        float targetAngle = Vector3.SignedAngle(lookDirection, targetDirection, Vector3.up);

        if (Mathf.Abs(targetAngle) > 0.1f)
        for (int i = 0; i < Frames; i++)
        {
            part.transform.Rotate(Vector3.up, targetAngle / Frames);
            yield return new WaitForSeconds(info.RotationSpeed / Frames);
        }
    }
    protected IEnumerator MoveByLineTo(Vector3 point)
    {
        float delta = (transform.position - point).magnitude;
        for (int i = 0; i < Frames; i++)
        {
            transform.Translate(delta / Frames * Vector3.forward, Space.Self);
            yield return new WaitForSeconds(info.MoveSpeed / Frames);
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
        attackedUnits.Clear();
    }
    #endregion
}
