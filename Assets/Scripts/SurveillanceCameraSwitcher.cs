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

    public void SetTimerValue(float v)
    {
        slider.value = v;
    }

    public float GetTimeToSwitchCamerasOff(){
        return timeToSwitchCamerasOff;
    }
    
}
