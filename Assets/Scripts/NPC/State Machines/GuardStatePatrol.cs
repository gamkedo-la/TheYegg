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
    [Header("Parent object for global patrol points")]
    [SerializeField] Transform patrolPointParent;
    [Header("Required components")] 
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] NPC nPC;


    //private
    private int currentPatrolPointIndex;
    public List<Transform> globalPatrolPoints = new List<Transform>();

    private void Awake() {
        if(globalPatrolPoints.Count <= 0){
            CreatePatrolPoints();
        }
    }

    public override void StartGuardState()
    {
        base.StartGuardState();

        navMeshAgent.autoBraking = false;
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = nPC.npcSpeed;
        GoToNextPatrolPoint();
    }

    private void CreatePatrolPoints(){
        //creates spawnpoints under a global parent for easier placing and debugging of patrol points
        foreach (Transform t in patrolPoints)
        {
            Transform globalPoint = Instantiate<Transform>(t, patrolPointParent, true);
            globalPatrolPoints.Add(globalPoint);
        }
    }

    private void GoToNextPatrolPoint()
    {
        if(patrolPoints.Count == 0){
            return;
        }
        navMeshAgent.destination = globalPatrolPoints[currentPatrolPointIndex].position;
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
        navMeshAgent.isStopped = true;
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

