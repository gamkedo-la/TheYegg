using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAlarm : MonoBehaviour
{
    // Used to handle all effects related to alarm system
    [SerializeField] ParticleSystem alarmParticleSystem;

    private void OnEnable() {
        AlarmSystemSwitch.OnAlarmTurnedOn += HandleAlarmTurnedOn;
        AlarmSystemSwitch.OnAlarmTurnedOff += HandleAlarmTurnedOff;
    }

    private void OnDisable() {
        AlarmSystemSwitch.OnAlarmTurnedOn -= HandleAlarmTurnedOn;
        AlarmSystemSwitch.OnAlarmTurnedOff -= HandleAlarmTurnedOff;
    }

    private void Start() {
        if(FindObjectOfType<LevelManager>().GetIsAlarmSystemOn()){
            alarmParticleSystem.Play();
        }
    }

    private void HandleAlarmTurnedOff(){
        alarmParticleSystem.Stop();
    }

    private void HandleAlarmTurnedOn(){
        alarmParticleSystem.Play();
    }


}
