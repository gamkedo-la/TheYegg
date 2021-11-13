using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{

    [Header("Action inputs")]
    [SerializeField] KeyCode incapacitateInput;
    [SerializeField] KeyCode equipDisguiseInput;
    [SerializeField] KeyCode openDoorInput;
    [SerializeField] KeyCode pickUpKeyInput;
    [SerializeField] KeyCode interactWithLevelSequenceInput;
    [SerializeField] KeyCode levelTransitionInput;
    [SerializeField] KeyCode surveillanceCameraSwitchInput;
    [SerializeField] KeyCode levelExitInput;
    [SerializeField] KeyCode alarmSystemSwitchInput;


    [Header("Required components")]
    [SerializeField] PlayerSFXHandler playerSFXHandler;
    [SerializeField] NPCHandler nPCHandler;
    [SerializeField] DisguiseHandler disguiseHandler;
    [SerializeField] KeyHandler keyHandler;
    [SerializeField] LevelSequenceHandler levelSequenceHandler;
    [SerializeField] LevelTransitionHandler levelTransitionHandler;
    [SerializeField] SurveillanceSystemHandler surveillanceSystemHandler;
    [SerializeField] LevelExitHandler levelExitHandler;
    [SerializeField] AlarmSystemHandler alarmSystemHandler;

    [Header("Interface to detection")]
    public bool isCompromisedDisguise = false;
    public bool isDoingIllegalAction = false;
    public bool isInRestrictedArea = false;

    //private
    private bool canOpenDoor;
    private float openDoorStartTime;
    private bool canInteract;
    private float interactStartTime;
    private float switchCameraTime;
    private bool canSwitchCamera;
    
    private void OnEnable() {
        AlarmSystemSwitch.OnAlarmTurnedOff += HandleAlarmTurnedOff;
        AlarmSystemSwitch.OnAlarmTurnedOn += HandleAlarmTurnedOn;
    }

    private void OnDisable(){
        AlarmSystemSwitch.OnAlarmTurnedOff -= HandleAlarmTurnedOff;
        AlarmSystemSwitch.OnAlarmTurnedOn -= HandleAlarmTurnedOn;
    }

    


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(equipDisguiseInput)){
           disguiseHandler.EquipDisguise(nPCHandler.GetClosestDownedNPC());
        }

        if(Input.GetKeyDown(incapacitateInput)){
            NPC npc = nPCHandler.GetClosestNPC();
            if(npc != null && npc.isNPCDown == false){
                npc.IncapacitateNPC();
            }
        }

        if(Input.GetKeyDown(openDoorInput)){
            if(keyHandler.StartOpenDoor() == true){
                openDoorStartTime = Time.time;
                canOpenDoor = true;
            } else {
                canOpenDoor = false;
            }
            
        }

        if(Input.GetKey(openDoorInput) && canOpenDoor){
            keyHandler.HandleDoorTimer(Time.time - openDoorStartTime);
        }

        if(Input.GetKeyUp(openDoorInput) && canOpenDoor){
            keyHandler.OpenDoor(Time.time - openDoorStartTime);
            openDoorStartTime = 0f;
        }


        if(Input.GetKeyDown(pickUpKeyInput)){
            keyHandler.PickUpKey();
        }

        if(Input.GetKeyDown(interactWithLevelSequenceInput)){
            if(levelSequenceHandler.StartInteraction() == true){
                interactStartTime = Time.time;
                canInteract = true;
            } else {
                canInteract = false;
            }
        }

        if(Input.GetKey(interactWithLevelSequenceInput) && canInteract){
            levelSequenceHandler.HandleTimer(Time.time - interactStartTime);
        }

        if(Input.GetKeyUp(interactWithLevelSequenceInput) && canInteract){
            levelSequenceHandler.InterAct(Time.time - interactStartTime);
            interactStartTime = 0f;
        }

        if(Input.GetKeyDown(levelTransitionInput)){
            levelTransitionHandler.UseLevelTransition();
        }

        if(Input.GetKeyDown(levelExitInput)){
            levelExitHandler.UseLevelExit();
        }

        if(Input.GetKeyDown(surveillanceCameraSwitchInput)){
            if(surveillanceSystemHandler.StartSurveillanceCameraSwitchHandling() == true){
                switchCameraTime = Time.time;
                canSwitchCamera = true;
            } else {
                canSwitchCamera = false;
            }
        }

        if(Input.GetKey(surveillanceCameraSwitchInput) && canSwitchCamera){
            surveillanceSystemHandler.HandleSwitchTimer(Time.time - switchCameraTime);
        }

        if(Input.GetKeyUp(surveillanceCameraSwitchInput) && canSwitchCamera){
            surveillanceSystemHandler.SwitchCameras(Time.time - switchCameraTime);
        }

        if(Input.GetKeyDown(alarmSystemSwitchInput)){
            alarmSystemHandler.SwitchAlarmOff();
        }

    }

    public void SetIsInRestrictedArea(bool t){
        isInRestrictedArea = t;
        if(t == true){
            playerSFXHandler.PlayPlayerHeartBeat();
        } else {
            playerSFXHandler.StopCurrentPlayerSFX();
        }
        
        //play SFX from PlayerSFXHandler
        
    }

    public void SetIsDisguiseCompromised(bool t){
        isCompromisedDisguise = t;
        //IF true, change display value
        
    }

    private void HandleAlarmTurnedOn()
    {
        SetIsInRestrictedArea(true);
    }

    private void HandleAlarmTurnedOff()
    {
        SetIsInRestrictedArea(false);
    }
}
