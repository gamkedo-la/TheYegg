using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] DoorKey correctKey;
 
    

    [Header("Required Components")]
    [SerializeField] Collider interactionAreaCollider;

    [Header("References to other Gameobjects")]
    [SerializeField] BoxCollider parentCollider;
    [SerializeField] Slider doorTimer;

    private LevelManager levelManager;
    private string id;

    private void Start() {
        //create ID for door based on object name and scene name
        id = string.Format("{0}-{1}", this.name, SceneManager.GetActiveScene().ToString());
        
        //check from levelmanager if the door has been opened already
        levelManager = FindObjectOfType<LevelManager>();
        if(!levelManager || levelManager == null){
            Debug.LogWarning("Door was unable to find a LevelManager in the scene!");
        }
        if(levelManager.GetIsDoorOpen(id)){
            Debug.Log("Door was Opened!");
            OpenDoor();
        }
    }

    public DoorKey GetCorrectDoorKey(){
        return correctKey;
    }

    public bool OpenDoor(){
        //disable the collider from the parent object
        parentCollider.enabled = false;
        // from collider and all its children
        Renderer[] doorRend = parentCollider.GetComponentsInChildren<Renderer>();
        for(int i=0; i<doorRend.Length;i++) {
            doorRend[i].enabled = false; // hide door
        }
        // any visuals from this door object's children
        doorRend = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < doorRend.Length; i++) {
            doorRend[i].enabled = false; // hide door
        }
        //TODO add an actual door and animations and sounds
        FindObjectOfType<LevelManager>().AddToOpenedDoors(id);
        return true;
    }

    public void SetDoorTimerValue(float f){
        doorTimer.value = f;
    }

}
