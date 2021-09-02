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


    [Header("Required components")]
    [SerializeField] NPCHandler nPCHandler;
    [SerializeField] DisguiseHandler disguiseHandler;
    [SerializeField] KeyHandler keyHandler;
    [SerializeField] SafeHandler safeHandler;

    [Header("Interface to detection")]
    public bool isCompromisedDisguise;
    public bool isDoingIllegalAction;

    //private
    private bool canOpenDoor;
    private float openDoorStartTime;
    private bool canOpenSafe;
    private float openSafeStartTime;
    
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

        if(Input.GetKeyDown(KeyCode.P)) {
            pauseOrResumeGame();
        }

    }

    // Pause or Resume Game
    void pauseOrResumeGame() 
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
}
