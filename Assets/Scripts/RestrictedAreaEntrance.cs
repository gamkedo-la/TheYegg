using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedAreaEntrance : MonoBehaviour
{
    
    private RestrictedAreaHandler restrictedAreaHandler;

    private void Start() {
        restrictedAreaHandler = GetComponentInParent<RestrictedAreaHandler>();
        if(!restrictedAreaHandler || restrictedAreaHandler == null){
            Debug.LogWarning("Restricted area entrance " + gameObject.name + " cannot find a restricted area handler in the parent object");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            restrictedAreaHandler.SetEntranceTime(this.gameObject, Time.time);
            restrictedAreaHandler.SetTriggered(this.gameObject);
        }
    }

}
