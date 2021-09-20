using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceSystemHandler : MonoBehaviour
{

    //this scripts handles interfacing with the surveillance system
    [Header("Surveillance System Settings")]
    [SerializeField] LayerMask interactLayerMask;

    private SurveillanceCameraSwitcher switcher;
    private float timeToSwitchCamerasOff;

    
    public bool StartSurveillanceCameraSwitchHandling()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach(Collider hit in hitColliders){
            if(hit.TryGetComponent<SurveillanceCameraSwitcher>(out switcher)){
                timeToSwitchCamerasOff = switcher.GetTimeToSwitchCamerasOff();
                return true;
            } else {
                switcher = null;
            }
        }
        return false;
    }

    public void SwitchCameras(float inputTime){
        if(inputTime > timeToSwitchCamerasOff){
            Debug.Log("Cameras are turned off!");
            if(switcher){
                
                switcher.SwitchCamerasOff();
            }
        }
    }
   
}
