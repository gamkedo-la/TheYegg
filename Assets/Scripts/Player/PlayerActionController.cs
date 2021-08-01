using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{

    [Header("Action inputs")]
    [SerializeField] KeyCode incapacitateInput;
    [SerializeField] KeyCode equipDisguiseInput;
    [SerializeField] KeyCode openDoorInput;


    [Header("Required components")]
    [SerializeField] NPCHandler nPCHandler;
    [SerializeField] DisguiseHandler disguiseHandler;
    [SerializeField] KeyHandler keyHandler;
    
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
            if(npc.isNPCDown == false){
                npc.IncapacitateNPC();
            }
            
        }

        if(Input.GetKeyDown(openDoorInput)){
            keyHandler.OpenDoor();
        }
    }
}
