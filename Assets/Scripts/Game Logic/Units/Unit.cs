using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.CanvasScaler;

public class Unit : MonoBehaviour
{
    protected static int Frames = 25;

    public UnitInfo info;
    public List<MeshRenderer> colorParts = new();

    protected Team team;
    protected VectorHex position;
    protected Vector3 scale;

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
        scale = transform.localScale;
    }
    public bool CanStayOnTile(HexTile tile)
    {
        if (!tile.HasUnit)
            return true;

        if (tile.Unit is Transport transport &&
            tile.Unit.Team == Team &&
            transport.CanBoard &&
            !info.Flying)
            return true;

        return false;
    }
    public bool CanMoveThroughTile(HexTile tile)
    {
        // Unit is enemy
        if (tile.HasUnit && tile.Unit.Team != team)
            return false;

        // Tile is obstacle and unit cant fly
        if (!info.Flying && tile.IsObstacle)
            return false;
        
        return true;
    }
    public bool CanAttackTile(HexTile tile)
    {
        // Unit doesnt exist
        if (!tile.HasUnit)
            return false;

        // Unit has same team
        if (tile.Unit.Team == team)
            return false;

        // Unit already attacked
        if (attackedUnits.Contains(tile.Position))
            return false;

        // Unit under another force field
        if (tile.Protected && tile.ProtectedBy != Game.Grid[position].ProtectedBy)
            return false;

        // Unit too close
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
    public void BoardTo(Transport to)
    {
        var path = FindPath(position, to.Position);

        void preAction()
        {
            movePoints = 0;
            position = VectorHex.UNSIGNED;
        }
        void postAction()
        {
        }

        StartCoroutine(Wrapper(
            preAction,
            () => AnimateBoard(path, to, 1),
            postAction
        ));
    }
    public void UnboardFrom(Transport from, VectorHex on)
    {
        List<VectorHex> path = FindPath(from.Position, on);

        void preAction()
        {
            movePoints -= path.Count;
            position = on;
        }
        void postAction()
        {
            SetGlobalPositionTo(Game.Grid[on]);
        }

        StartCoroutine(Wrapper(
            preAction,
            () => AnimateBoard(path, from, 0),
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
                GlobalEventManager.UnitDied.Invoke(position, team);
        }

        StartCoroutine(Wrapper(
            preAction,
            () => AnimateDamage(),
            postAction
        ));
    }
    #endregion

    #region Virtuals
    protected virtual IEnumerator AnimateMove(List<VectorHex> path, int scaleOption=-1)
    {
        yield return MoveByPath(path, scaleOption);
    }
    protected IEnumerator AnimateBoard(List<VectorHex> path, Transport transport, int scaleOption)
    {
        yield return transport.MoveDown();
        transport.onBoardCount++;
        yield return AnimateMove(path, scaleOption);
        transport.onBoardCount--;
        yield return transport.MoveUp();
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
    protected virtual void SetTeam(Team team)
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
    #endregion

    #region Operations
    protected IEnumerator Wrapper(Action preAction, Func<IEnumerator> animation, Action postAction)
    {
        preAction();
        yield return animation();
        postAction();
    }
    public void SetGlobalPositionTo(HexTile to)
    {
        transform.position = to.gameObject.transform.position;
        transform.position += Vector3.up * info.OffsetOverTile;
    }
    protected IEnumerator MoveByPath(List<VectorHex> path, int scaleOption=-1)
    {
        int i = 0;
        int step = scaleOption == 0 ? 0 : path.Count - 1;

        foreach (VectorHex p in path)
        {
            Vector3 tilePos = Game.Grid[p].transform.position;

            yield return RotateTo(transform, tilePos);

            if (scaleOption != - 1 && i == step)
                StartCoroutine(AnimateSwitchScale());

            yield return MoveByLineTo(tilePos);

            i++;
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
    protected IEnumerator AnimateSwitchScale()
    {
        float targetScale = transform.localScale.x < scale.x ? scale.x : 0f;
        float delta = targetScale - transform.localScale.x;

        for (int i = 0; i < Frames; i++)
        {
            transform.localScale += delta / Frames * Vector3.one;
            yield return new WaitForSeconds(info.MoveSpeed / Frames);
        }

        transform.localScale = targetScale * Vector3.one;
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
