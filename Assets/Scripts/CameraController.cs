using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float zoomInSpeed = 1f;
    [SerializeField] float zoomInLimit = 3f;
    [SerializeField] float zoomInDuration = 2f;
    [SerializeField] float zoomOutDuration = 2f;
    [Tooltip("This is fetched from the cinemachine component on start")]
    [SerializeField] float zoomOutLimit;
    [SerializeField] float playerIdleThreshold = 2f;
    
    private CinemachineVirtualCamera cm;
    private CinemachineOrbitalTransposer cmot;
    private PlayerMovement playerMovement;
    private float zoom;
    private bool idleTimerStarted;
    private float idleStartTime;
    private bool isMovement = false;
    private float yVelocity = 0f;




    // Start is called before the first frame update
    void Start()
    {

        //get the upper zoom limit from the cinemachine component
        cm = GetComponent<CinemachineVirtualCamera>();
        cm.m_Follow = FindObjectOfType<PlayerMovement>().transform;
        cmot = cm.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        //get y offset from virtual camera body
        zoomOutLimit = cmot.m_FollowOffset.y;
        //zoomOutLimit = cm.m_Lens.FieldOfView;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        zoom = zoomOutLimit;
    }

    private void Update() {
        isMovement = playerMovement.GetPlayerMovementMagnitude() != 0f;
        
        if(isMovement){
            idleTimerStarted = false;
            //zoomInFov = zoomInLimit;
        }

        if(!isMovement && idleTimerStarted == false){
            //movement stopped, start idle timer
            //start timer for idling
            idleStartTime = Time.time;
            idleTimerStarted = true;
            //zoomOutFov = zoomOutLimit;
        }

        if(Time.time - idleStartTime > playerIdleThreshold && playerMovement.GetPlayerMovementMagnitude() == 0f){
            zoom = Mathf.SmoothDamp(zoom, zoomInLimit, ref yVelocity, zoomInDuration);
            cmot.m_FollowOffset.y  = zoom;
        } else {
            zoom = Mathf.SmoothDamp(zoom, zoomOutLimit, ref yVelocity, zoomOutDuration);
            cmot.m_FollowOffset.y = zoom;
        }
    }


}
