using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHandler : MonoBehaviour
{

    [Header("Interaction area settings")]
    [SerializeField] LayerMask interactLayerMask;

    [Header("Collected Keys")]
    public List<DoorKey> keys = new List<DoorKey>();

    //private
    private bool isNearDoor = false;
    private bool isNearKey = false;

    public Door door;
    public Key key;


    public void OpenDoor()
    {
        //if close enough to a door
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach(Collider hit in hitColliders){
            if(hit.gameObject.TryGetComponent<Door>(out door)){
                DoorKey correctKey = door.GetCorrectDoorKey();
                foreach (DoorKey key in keys)
                {
                    if(key == correctKey){
                        door.OpenDoor();
                    }
                }
            } else {
                Debug.Log("No door in vicinity");
            }
        }
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
