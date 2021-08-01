using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public DoorKey correctKey;

    [Header("Required Components")]
    [SerializeField] Collider interactionAreaCollider;

    [Header("References to other Gameobjects")]
    [SerializeField] BoxCollider parentCollider;

    public DoorKey GetCorrectDoorKey(){
        return correctKey;
    }

    public void OpenDoor(){
        //disable the collider from the parent object
        parentCollider.enabled = false;
        //TODO add an actual door and animations and sounds
    }

}
