using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateIncapacitated : GuardState
{

    [Header("Incapacitated settings")]
    [SerializeField] float timeToReset = 5f;

    private float startTime;
    private NPC npc;
    public bool canExitState = false;

    public override void StartGuardState(){
        //start a timer which is compared to timeToReset which will bring the guard back to life
        startTime = Time.time;
        npc = gameObject.GetComponent<NPC>();
        if(!npc || npc == null){
            Debug.LogWarning("Guard State Incapacitated could not find an NPC component");
        }
    }

    public override void RunGuardState(){
        base.RunGuardState();
        if(Time.time - startTime > timeToReset){
            //reset to default state
            Debug.Log("Reactivating guard");
            guardFSM.PushState(guardFSM.defaultState);
            EndGuardState();
            canExitState = true;
        } else {
            canExitState = false;
        }
    }

    public override void EndGuardState(){
        //this may be called from other methods, therefore reactivate here too
        if(canExitState){
            npc.ReactivateNPC();
            base.EndGuardState();
        }
    }
}
