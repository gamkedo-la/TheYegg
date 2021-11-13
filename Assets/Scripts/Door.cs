using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] DoorKey correctKey;
 
    

    [Header("Required Components")]
    [SerializeField] Collider interactionAreaCollider;

    [Header("References to other Gameobjects")]
    [SerializeField] BoxCollider parentCollider;
    [SerializeField] Slider doorTimer;

    public DoorKey GetCorrectDoorKey(){
        return correctKey;
    }

    public bool OpenDoor(){
        //disable the collider from the parent object
        parentCollider.enabled = false;
        //TODO add an actual door and animations and sounds
        return true;
    }

    public void SetDoorTimerValue(float f){
        doorTimer.value = f;
    }

}
