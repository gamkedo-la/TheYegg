using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardStatePatrol : GuardState
{

    [Header("Patrol pathfinding settings")]
    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] float allowedDistanceFromPoint = .5f;
    [SerializeField] float patrolSpeed = 5f;
    [Header("Required components")] 
    [SerializeField] NavMeshAgent navMeshAgent;


    //private
    private int currentPatrolPointIndex;

    public override void StartGuardState()
    {
        base.StartGuardState();
        navMeshAgent.autoBraking = false;
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = patrolSpeed;
        GoToNextPatrolPoint();
    }

    private void GoToNextPatrolPoint()
    {
        if(patrolPoints.Count == 0){
            return;
        }
        navMeshAgent.destination = patrolPoints[currentPatrolPointIndex].position;
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Count;
    }

    public override void RunGuardState()
    {
        base.RunGuardState();
        //TODO create pathfinding between patrolpoints so that we can set random patrolpoints when player is detected
        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance < allowedDistanceFromPoint){
            //has reached a patrol point, stay idle for x seconds
            guardFSM.PushState(guardFSM.idleState);
            navMeshAgent.isStopped = true;
            EndGuardState();
            //GoToNextPatrolPoint();
        }
    }

    public override void EndGuardState()
    {
        base.EndGuardState();
        
    }

    private void OnCollisionStay(Collision other) {
        if(other.gameObject.CompareTag("Floor")){
            Debug.Log("Guard is on floor");
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;                
        }
    }
}

