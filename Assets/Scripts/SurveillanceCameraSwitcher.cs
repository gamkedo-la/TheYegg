using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCameraSwitcher : MonoBehaviour
{

    private LevelManager levelManager;
    [SerializeField] float timeToSwitchCamerasOff = 1f;
    [SerializeField] GameObject camerasOnObject;
    [SerializeField] GameObject camerasOffObject;

    private ScoreKeeper scoreKeeper;


    private void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(!scoreKeeper || scoreKeeper == null){
            Debug.LogWarning("No ScoreKeeper found in scene for " + gameObject.name);
        }
    }

    public void SwitchCamerasOff(){
        Debug.Log("Switching cameras off");
        levelManager.SetCamerasDisabled(true);
        scoreKeeper.SetIsDetectedByCameras(false);
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

    public float GetTimeToSwitchCamerasOff(){
        return timeToSwitchCamerasOff;
    }
    
}
