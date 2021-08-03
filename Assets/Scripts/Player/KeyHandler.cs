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
    private bool isNearKey = false;

    public Door door;
    public Key key;

    private void OnTriggerStay(Collider other) {
        //Debug.Log("Trigger collission with " + other.name);
        if(other.TryGetComponent<Door>(out door)){
            isNearDoor = true;
        }

        if(other.TryGetComponent<Key>(out key)){
            isNearKey = true;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        //Debug.Log("Trigger exited!");
        if(other.TryGetComponent<Door>(out door)){
            isNearDoor = false;
            door = null;
        }

        if(other.TryGetComponent<Key>(out key)){
            isNearKey = false;
            key = null;
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

    public void PickUpKey(){
        if(isNearKey && key != null){
            keys.Add(key.PickUpKey());
            key.DestroyKey();
        }
    }
}
