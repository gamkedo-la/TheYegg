using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurveillanceCameraSwitcher : MonoBehaviour
{

    private LevelManager levelManager;
    [SerializeField] float timeToSwitchCamerasOff = 1f;
    [SerializeField] GameObject camerasOnObject;
    [SerializeField] GameObject camerasOffObject;
    [SerializeField] Slider slider;

    private ScoreKeeper scoreKeeper;
    [SerializeField] GameObject spawnWhenSwitchedOff;

    private void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(!scoreKeeper || scoreKeeper == null){
            Debug.LogWarning("No ScoreKeeper found in scene for " + gameObject.name);
        }
    }

    public void SwitchCamerasOff(){

        // spawn an optional prefab
        if (spawnWhenSwitchedOff) {
            Debug.Log("Spawning camera off fx at "+transform.position.ToString());
            Instantiate(spawnWhenSwitchedOff, transform.position, Quaternion.Euler(90f,0f,0f));
        }

        Debug.Log("Switching cameras off");
        levelManager.SetCamerasDisabled(true);
        scoreKeeper.SetIsDetectedByCameras(false);
        WorldInteractable worldInteractable;
        if(TryGetComponent<WorldInteractable>(out worldInteractable))
        {
            worldInteractable.SetPromptObjectsActive(false);
        }
        BoxCollider boxCollider;
        if(TryGetComponent<BoxCollider>(out boxCollider))
        {
            boxCollider.enabled = false;
        }
        //get all cameras in current scene and turn them off
        camerasOnObject.SetActive(false);
        camerasOffObject.SetActive(true);
        SurveillanceCamera[] cams = FindObjectsOfType<SurveillanceCamera>();
        foreach (SurveillanceCamera cam in cams)
        {
            cam.DisableCamera();
            cam.GetComponentInParent<SurveillanceCameraRotator>().DisableCameraRotator();
         
        }

    }

    public void SetTimerValue(float v)
    {
        slider.value = v;
    }

    public float GetTimeToSwitchCamerasOff(){
        return timeToSwitchCamerasOff;
    }
    
}
