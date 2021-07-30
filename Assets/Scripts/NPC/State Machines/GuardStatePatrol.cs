using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStatePatrol : GuardState
{

    [SerializeField] List<Transform> patrolPoints;

    public override void StartGuardState()
    {
        base.StartGuardState();
    }

    public override void RunGuardState()
    {
        base.RunGuardState();
        //Debug.Log(name + " is patrolling");
        //TODO create pathfinding between patrolpoints so that we can set random patrolpoints when player is detected
    }

    public override void EndGuardState()
    {
        base.EndGuardState();
    }




}
