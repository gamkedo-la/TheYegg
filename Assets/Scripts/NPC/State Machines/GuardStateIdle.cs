using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateIdle : GuardState
{

    [SerializeField] float idleTimeInSeconds = 3f;
    [SerializeField] AudioClip walkieTalkieAudio;
    public float timeSpentIdle = 0f;
    public DistanceSFXPlayer distanceSFXPlayer;
    private bool walkieTalkiePlayed = false;

    public override void StartGuardState()
    {
        //start counting down seconds that the guard stays idle
        timeSpentIdle = 0f;
        //if the FOV is not active, activate it
        //play walkie talkie audio
        walkieTalkiePlayed = false;
        
    }

    public override void RunGuardState()
    {
        base.RunGuardState();
        CountTimeIdle();
        if(!walkieTalkiePlayed){
            distanceSFXPlayer.PlayOneShotClip(walkieTalkieAudio);
            walkieTalkiePlayed = true;
        }
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
