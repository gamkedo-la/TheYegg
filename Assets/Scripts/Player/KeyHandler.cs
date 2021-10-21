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
    [Header("Door Audio Settings")]
    public List<AudioClip> doorAudioClips = new List<AudioClip>();
    public AudioSource audioSource;

    [Header("Collected Keys")]
    public List<DoorKey> keys = new List<DoorKey>();
    [Tooltip("Number of lockpicks left")]
    public int lockpickCount;
    public int lockpickCountAtStartOfLevel = 2;

    //private
    private bool hasMatchingKey = false;

    public Door door;
    public Key key;

    private void Start() {
        //GameObject.FindObjectOfType<HUDHandler>().SetLockPickCount(lockpickCount);
        //foreach (DoorKey key in keys)
        //{
           // GameObject.FindObjectOfType<HUDHandler>().SetCollectedKeys(key.ToString());
        //}
        
    }

    public string GetKeyString()
    {
        String s = "";
        foreach (DoorKey key in keys)
        {
            if(s.Length < 1){
                s = key.ToString();
            } else {
                s = s + ", " + key.ToString();
            }
            
        }

        return s;
    }

    public void OpenDoor(float inputTime)
    {
        if(hasMatchingKey){
            //compare with time required to open with key
            if(inputTime > timeToOpenWithKey){
                door.OpenDoor();
                hasMatchingKey = false;
                audioSource.PlayOneShot(doorAudioClips[UnityEngine.Random.Range(0, doorAudioClips.Count)]);
            }
        } else {
            if(inputTime > timeToOpenWithLockpick){
                door.OpenDoor();
                lockpickCount -= 1;
                GameObject.FindObjectOfType<HUDHandler>().SetLockPickCount(lockpickCount);
                audioSource.PlayOneShot(doorAudioClips[UnityEngine.Random.Range(0, doorAudioClips.Count)]);
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
                        hasMatchingKey = true;
                        return true;
                    }
                }

                if(hasMatchingKey == false){
                    if(lockpickCount > 0){
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
                if(!keys.Contains(key.GetKeyType())){
                    keys.Add(key.GetKeyType());
                    GameObject.FindObjectOfType<HUDHandler>().SetCollectedKeys(key.GetKeyType().ToString());
                }
                key.DestroyKey();
            } else {
                Debug.Log("No key in vicinity");
            }
        }
    }

    public int GetLockPickCount(){
        return lockpickCount;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    public void ResetKeyHandler(){
        lockpickCount = lockpickCountAtStartOfLevel;
        HUDHandler hUDHandler = GameObject.FindObjectOfType<HUDHandler>();
        if(hUDHandler){
            hUDHandler.SetLockPickCount(lockpickCount);
            keys.Clear();
            foreach (DoorKey key in keys){
            hUDHandler.SetCollectedKeys(key.ToString());
            }
        }
        
    }
}
