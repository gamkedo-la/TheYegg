using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{

    [Header("Door Interaction Settings")]
    [SerializeField] LayerMask interactLayerMask;
    [SerializeField] float timeToOpenWithKey;
    [SerializeField] float timeToOpenWithLockpick;

    [Header("Collected Keys")]
    public List<DoorKey> keys = new List<DoorKey>();
    [Tooltip("Number of lockpicks left")]
    public int lockpickCount;

    //private
    private bool isNearDoor = false;
    private bool isNearKey = false;
    private bool hasMatchingKey = false;
    private float startTime = 0f;
    private float stopTime = 0f;

    public Door door;
    public Key key;


    public void OpenDoor(float inputTime)
    {
        if(hasMatchingKey){
            //compare with time required to open with key
            if(inputTime > timeToOpenWithKey){
                door.OpenDoor();
                hasMatchingKey = false;
            }
        } else {
            if(inputTime > timeToOpenWithLockpick){
                door.OpenDoor();
                lockpickCount -= 1;
            }
        }

        
    }

    public bool StartOpenDoor(){
        //checks if the player has an available method of opening a door. If so, the playeractioncontroller checks if the player has pressed the input long enough, which is fed back to OpenDoor as float
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach(Collider hit in hitColliders){
            if(hit.gameObject.TryGetComponent<Door>(out door)){
                DoorKey correctKey = door.GetCorrectDoorKey();
                foreach (DoorKey key in keys)
                {
                    if(key == correctKey){
                        Debug.Log("Matching key found");
                        hasMatchingKey = true;
                        return true;
                    }
                }

                if(hasMatchingKey == false){
                    if(lockpickCount > 0){
                        Debug.Log("Need to use lockpicks");
                        hasMatchingKey = false;
                        return true;
                    }
                }
            } 
        }
        return false;
    }

    public void PickUpKey(){
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach(Collider hit in hitColliders){
            if(hit.gameObject.TryGetComponent<Key>(out key)){
                keys.Add(key.GetKeyType());
                key.DestroyKey();
            } else {
                Debug.Log("No key in vicinity");
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
