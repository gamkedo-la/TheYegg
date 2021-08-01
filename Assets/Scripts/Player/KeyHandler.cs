using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{

    [Header("Collected Keys")]
    public List<DoorKey> keys = new List<DoorKey>();

    [Header("Required Components")]
    [SerializeField] Collider actionCollider;

    //private
    private bool isNearDoor = false;
    private Door door;

    private void OnTriggerStay(Collider other) {
        if(other.TryGetComponent<Door>(out door)){
            isNearDoor = true;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<Door>(out door)){
            isNearDoor = false;
        }
    }

    public void OpenDoor()
    {
        //if close enough to a door
        if(isNearDoor && door != null){
            //get correct door key from door
            DoorKey correctKey = door.GetCorrectDoorKey();
            foreach (DoorKey key in keys)
            {
                if(key == correctKey){
                    Debug.Log("Matching key found, opening door");
                    door.OpenDoor();
                }
            }
        } else {
            Debug.Log("Not able to open door");
        }
    }
}
