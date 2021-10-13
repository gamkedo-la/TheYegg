using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSystemHandler : MonoBehaviour
{
    [SerializeField] LayerMask interactLayerMask;

    public void SwitchAlarmOff()
    {
        AlarmSystemSwitch alarmSystemSwitch;
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach(Collider hit in hitColliders){
            if(hit.gameObject.TryGetComponent<AlarmSystemSwitch>(out alarmSystemSwitch)){
                alarmSystemSwitch.SwitchAlarmOff();
            }
        }
    }
}
