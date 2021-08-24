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


    [Header("Required components")]
    [SerializeField] NPCHandler nPCHandler;
    [SerializeField] DisguiseHandler disguiseHandler;
    [SerializeField] KeyHandler keyHandler;

    [Header("Interface to detection")]
    public bool isCompromisedDisguise;
    public bool isDoingIllegalAction;

    public float openDoorStartTime;

    //private
    private bool canOpenDoor;
    
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
            Debug.Log("Tried time was " + (Time.time - openDoorStartTime).ToString());
            openDoorStartTime = 0f;
        }


        if(Input.GetKeyDown(pickUpKeyInput)){
            keyHandler.PickUpKey();
        }
    }
}
