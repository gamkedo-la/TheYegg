using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateIdle : GuardState
{

    [SerializeField] float idleTimeInSeconds = 3f;

    public float timeSpentIdle = 0f;

    public override void StartGuardState()
    {
        //start counting down seconds that the guard stays idle
        timeSpentIdle = 0f;
    }

    public override void RunGuardState()
    {
        base.RunGuardState();
        CountTimeIdle();
    }

    private void CountTimeIdle()
    {
        timeSpentIdle += Time.deltaTime;
    }

    public override void GuardStateCheck()
    {

        base.GuardStateCheck();
        if(timeSpentIdle >= idleTimeInSeconds){
            guardFSM.PushState(guardFSM.patrolState);
            EndGuardState();
        }
    }
}
