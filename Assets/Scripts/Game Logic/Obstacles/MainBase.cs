using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBase : Factory
{
    [SerializeField] private Team initTeam;

    protected override Team Default => initTeam;
}
