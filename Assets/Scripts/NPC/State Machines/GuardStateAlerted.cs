using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStateAlerted : GuardState
{
    [Header("Alert state settings")]
    public Vector3 lastKnownLocation;
    public bool isAlertedByAnother;
    [SerializeField] float alertRadius;
    [SerializeField] LayerMask alertLayerMask;
    [SerializeField] [Range(0f, 30f)] float alertMaxTime;
    [SerializeField] float allowedDistanceFromPlayer = 1f;

    [Header("Required Components")]
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] NPC nPC;

    //private
    private float alertStartTime;

    public override void StartGuardState()
    {
        base.StartGuardState();
        //alert all guards inside alerting radius IF this guard is the first to detect something
        if(!isAlertedByAnother){
            GetGuardsToAlert();
        }
        StartAlertTimer();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = nPC.npcSpeed;
    }

    public override void RunGuardState(){
        base.RunGuardState();
        //TODO move to last know location based on lastKnownLocation
        MoveToLastKnownLocation();
        HandleAlertTimer();
    }

    public override void EndGuardState()
    {
        SetIsAlertedByAnother(false);
        SetLastKnownLocation(Vector3.zero);
        alertStartTime = 0f;
        navMeshAgent.isStopped = true;
        base.EndGuardState();
    }
        
    private void GetGuardsToAlert()
    {
        GuardFSM[] allGuards = FindObjectsOfType<GuardFSM>();
        foreach(GuardFSM g in allGuards){
            if(Vector3.Distance(g.transform.position, lastKnownLocation) <= alertRadius){
                //raycast to the guard in the radius to see if there is a wall in between
                Vector3 dirToGuard = g.transform.position - transform.position;
                Ray ray = new Ray(transform.position, (dirToGuard));
                if(Physics.Raycast(ray, alertRadius, alertLayerMask)){
                    //alert others
                    if(g.activeState != g.alertState){
                        g.gameObject.GetComponent<GuardStateAlerted>().SetIsAlertedByAnother(true);
                        g.PushState(g.alertState);
                        g.activeState.EndGuardState();
                    }
                    
                }
            }
        }
    }

    public void SetLastKnownLocation(Vector3 loc){
        lastKnownLocation = loc;
    }

    public void SetIsAlertedByAnother(bool t){
        isAlertedByAnother = t;
    }

    private void StartAlertTimer()
    {
        //every time this guard is alerted, restart timer
        alertStartTime = Time.time;
    }

    private void HandleAlertTimer()
    {
        //if alert time runs out, return to default state
        if(Time.time - alertStartTime > alertMaxTime){
            guardFSM.PushState(guardFSM.defaultState);
            EndGuardState();
        } 
    }

    private void MoveToLastKnownLocation()
    {
        navMeshAgent.destination = lastKnownLocation;
        if(navMeshAgent.remainingDistance < allowedDistanceFromPlayer){
            //move to new state where the player is caught or game over
            navMeshAgent.isStopped = true;
        } else {
            navMeshAgent.isStopped = false;
        }
    }

}
