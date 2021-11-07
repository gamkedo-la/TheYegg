using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldInteractable : MonoBehaviour
{
    //this class shows a TMPro text once the player is inside the trigger area

    public TextMeshPro promptTextObject;

    public string promptText;

    private void Start() {
        promptTextObject.text = promptText;
        promptTextObject.gameObject.SetActive(false);
        RotateToWorldZ();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            //show TMPro object
            promptTextObject.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            promptTextObject.gameObject.SetActive(false);
        }
    }

    private void RotateToWorldZ(){
        promptTextObject.gameObject.transform.SetPositionAndRotation(promptTextObject.gameObject.transform.position, Quaternion.LookRotation(Vector3.down, Vector3.up));
    }


}
