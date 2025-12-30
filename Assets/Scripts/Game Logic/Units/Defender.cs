using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Defender : Unit
{
    [SerializeField] private GameObject tower;
    [SerializeField] private GameObject sphere;
    [SerializeField] private Animator animator;

    private HashSet<VectorHex> protectedArea = new();

    public bool CanActivateField => movePoints == info.MovementDistance && !TooCloseToOtherField;
    private bool TooCloseToOtherField
    {
        get
        {
            foreach (var pos in Game.Grid.GetRing(new() { position }, 2))
                if (Game.Grid[pos].Protected)
                    return true;

            return false;
        }
    }

    public void SetField(bool active)
    {
        animator.SetBool("FieldActive", active);

        if (active)
            SetProtections();
        else
            UnsetProtections();
    }

    #region Overrides
    protected override IEnumerator AnimateMove(List<VectorHex> path)
    {
        SetField(false);
        yield return base.AnimateMove(path);
    }
    protected override IEnumerator AnimateDamage()
    {
        SetField(false);
        yield return base.AnimateDamage();
    }
    protected override void SetTeam(Team team)
    {
        base.SetTeam(team);

        if (team == Team.Player)
            sphere.GetComponentInChildren<MeshRenderer>().material = Game.Art.PlayerSphere;
        else
            sphere.GetComponentInChildren<MeshRenderer>().material = Game.Art.EnemySphere;
    }
    #endregion

    #region Operations
    private void SetProtections()
    {
        var ring = Game.Grid.GetRing(new() { position }, 2);
        protectedArea.UnionWith(ring);

        foreach (var pos in ring)
            Game.Grid[pos].SetProtection(this);
    }
    private void UnsetProtections()
    {
        foreach (var pos in protectedArea)
            Game.Grid[pos].UnsetProtection();

        protectedArea.Clear();
    }
    #endregion
}
