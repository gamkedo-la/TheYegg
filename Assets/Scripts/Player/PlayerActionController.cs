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
    [SerializeField] KeyCode openSafeInput;
    [SerializeField] KeyCode levelTransitionInput;
    [SerializeField] KeyCode surveillanceCameraSwitchInput;
    [SerializeField] KeyCode levelExitInput;
    [SerializeField] KeyCode alarmSystemSwitchInput;


    [Header("Required components")]
    [SerializeField] PlayerSFXHandler playerSFXHandler;
    [SerializeField] NPCHandler nPCHandler;
    [SerializeField] DisguiseHandler disguiseHandler;
    [SerializeField] KeyHandler keyHandler;
    [SerializeField] SafeHandler safeHandler;
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
    private bool canOpenSafe;
    private float openSafeStartTime;
    private float switchCameraTime;
    private bool canSwitchCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

        if(Input.GetKeyUp(openDoorInput) && canOpenDoor){
            keyHandler.OpenDoor(Time.time - openDoorStartTime);
            openDoorStartTime = 0f;
        }


        if(Input.GetKeyDown(pickUpKeyInput)){
            keyHandler.PickUpKey();
        }

        if(Input.GetKeyDown(openSafeInput)){
            if(safeHandler.StartOpenSafe() == true){
                openSafeStartTime = Time.time;
                canOpenSafe = true;
            } else {
                canOpenSafe = false;
            }
        }

        if(Input.GetKeyUp(openSafeInput) && canOpenSafe){
            safeHandler.OpenSafe(Time.time - openSafeStartTime);
            openSafeStartTime = 0f;
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

        if(Input.GetKeyUp(surveillanceCameraSwitchInput)){
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
}
