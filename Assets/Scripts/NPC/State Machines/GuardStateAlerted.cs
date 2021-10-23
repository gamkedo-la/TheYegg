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
    [SerializeField] GuardAnimationController animationController;
    [SerializeField] DistanceSFXPlayer distanceSFXPlayer;

    //private
    private float alertStartTime;
    private LevelManager levelManager;
    private AlarmSystemSwitch alarmSystemSwitch;


    public override void StartGuardState()
    {
        base.StartGuardState();
        //alert all guards inside alerting radius IF this guard is the first to detect something
        levelManager = FindObjectOfType<LevelManager>();
        if(!levelManager || levelManager == null){
            Debug.LogWarning("Guard State Alerted could not find a levelmanager!");
        }
        alarmSystemSwitch = FindObjectOfType<AlarmSystemSwitch>();
        if(!alarmSystemSwitch || alarmSystemSwitch == null){
            Debug.Log("Guard State Alerted could not find an AlarmSystemSwitch, and that is ok if intentional!");
        }
        if(!isAlertedByAnother){
            GetGuardsToAlert();
            GoToAlarmSwitch();
        }
        StartAlertTimer();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = nPC.npcSpeed;
        //TODO add some sprite or other visual to show that this guard is alerted
    }

    private void GoToAlarmSwitch()
    {
        if(levelManager.GetIsAlarmSystemOn() == false && alarmSystemSwitch){
            //activate guard state which goes to the alarm switch and then to alert from there
            guardFSM.PushState(guardFSM.triggerAlarmState);
            guardFSM.activeState.EndGuardState();
        }
    }

    public override void RunGuardState(){
        base.RunGuardState();
        MoveToLastKnownLocation();
        HandleAlertTimer();
        TryCatchPlayer();
    }

    public override void EndGuardState()
    {
        SetIsAlertedByAnother(false);
        SetLastKnownLocation(Vector3.zero);
        alertStartTime = 0f;
        navMeshAgent.isStopped = true;
        animationController.SetIsWalking(false);
        distanceSFXPlayer.SetPlayAudio(false);
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
        
        if(lastKnownLocation != Vector3.zero){
            animationController.SetIsWalking(true);
            distanceSFXPlayer.SetPlayAudio(true);
            navMeshAgent.destination = lastKnownLocation;
            if(navMeshAgent.remainingDistance <= allowedDistanceFromPlayer){
                //move to new state where the player is caught or game over
                navMeshAgent.isStopped = true;
                animationController.SetIsWalking(false);
                TryCatchPlayer();
            } else {
                navMeshAgent.isStopped = false;
            }
        }
    }

    private void TryCatchPlayer()
    {
        //tell level manager that game is over
        if(Vector3.Distance(FindObjectOfType<PlayerMovement>().transform.position, this.transform.position) <= allowedDistanceFromPlayer){
            Debug.Log("Player is caught!");
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            levelManager.StartGameOver();
        }
    }
}
