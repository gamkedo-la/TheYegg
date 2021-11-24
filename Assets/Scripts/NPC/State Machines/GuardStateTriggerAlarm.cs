using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStateTriggerAlarm : GuardState
{


    private NavMeshAgent navMeshAgent;
    private AlarmSystemSwitch alarmSystemSwitch;
    [SerializeField] float allowedDistanceFromAlarm = .2f;
    [SerializeField] GuardAnimationController animationController;

    public override void StartGuardState()
    {
        base.StartGuardState();
        Debug.Log("Starting Trigger Alarm State!");
        navMeshAgent = GetComponent<NavMeshAgent>();
        if(!navMeshAgent || navMeshAgent == null){
            Debug.LogWarning("Guard State Trigger Alarm could not find a navmeshagent!");
        } else {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = GetComponent<NPC>().npcSpeed;
        }
        alarmSystemSwitch = FindObjectOfType<AlarmSystemSwitch>();
        if(!alarmSystemSwitch || alarmSystemSwitch == null){
            Debug.LogWarning("Guard State Trigger Alarm could not find an AlarmSystemSwitch!");
            EndGuardState();
        }
        else{
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.CalculatePath(alarmSystemSwitch.transform.position, path);
        }
        
    }

    public override void RunGuardState()
    {
        base.RunGuardState();
        if(navMeshAgent.isStopped){
            navMeshAgent.isStopped = false;
            animationController.SetIsWalking(true);
        }
        
        navMeshAgent.SetDestination(alarmSystemSwitch.transform.position);
        Debug.Log("Running Trigger State! Destination is " + navMeshAgent.destination + " and path is " + navMeshAgent.hasPath + " and speed is " + navMeshAgent.speed + " and agent is " + navMeshAgent.isStopped);
        if(Vector3.Distance(transform.position, alarmSystemSwitch.transform.position) <= allowedDistanceFromAlarm){
            Debug.Log("Got here!");
            alarmSystemSwitch.SwitchAlarmOn();
            //if there isn't an alert state in the stack, push alertstate
            if(!guardFSM.stateStack.Contains(guardFSM.alertState)){
                guardFSM.PushState(guardFSM.alertState);
            }
            
            EndGuardState();
        }
    }

    public override void EndGuardState()
    {
        
        base.EndGuardState();

    }
}
