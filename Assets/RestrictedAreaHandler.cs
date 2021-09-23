using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedAreaHandler : MonoBehaviour
{
    
    [SerializeField] GameObject positiveZGate;
    [SerializeField] GameObject negativeZGate;
    [Tooltip("Time in seconds for checking if player has entered a restricted area")]
    [SerializeField] float restrictedAreaCheckInterval;
    [SerializeField] bool restrictedAreaIsInZPos;
    
    
    private float positiveZExitTime;
    private float negativeZExitTime;
    private bool posZisTriggered = false;
    private bool negZisTriggered = false;


    public bool latestMoveDirIsZPos = false;

    public void SetEntranceTime(GameObject entrance, float time){
        if(entrance == positiveZGate){
            positiveZExitTime = time;
        } else if(entrance == negativeZGate){
            negativeZExitTime = time;
        }
        
    }

    public void SetTriggered(GameObject gate){
        //only check movement direction after both triggers have been triggered
        if(gate == positiveZGate){
            posZisTriggered = true;
        } else if(gate == negativeZGate){
            negZisTriggered = true;
        }

        if(posZisTriggered && negZisTriggered){
            CheckMovementDirection();
            posZisTriggered = false;
            negZisTriggered = false;
        }
    }

    private void CheckMovementDirection()
    {
        //while(true){
        PlayerActionController player = FindObjectOfType<PlayerActionController>();
        if(!player || player == null){
            Debug.LogWarning("Restricted area handler could not find a player!");
        } else {
            if(positiveZExitTime < negativeZExitTime){
                 //moved to the negative z side of the object
                latestMoveDirIsZPos = false;
            } else if (positiveZExitTime > negativeZExitTime){
                //moved to the positive z side of the object
                latestMoveDirIsZPos = true;
            }
            if(latestMoveDirIsZPos == restrictedAreaIsInZPos){
                //if the restricted area is in the moved direction, set player variables to reflect that the player is in a restricted area
                player.isInRestrictedArea = true;
            } else {
                player.isInRestrictedArea = false;
            }
        }
 
    }   
}
