using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSystemSwitch : MonoBehaviour
{
    private LevelManager levelManager;

    //assuming we have multiple wall alarms that need to be turned off in level, make an event for turning them off
    //event telling that the alarm is turned off or on
    public delegate void AlarmTurnedOn();
    public static event AlarmTurnedOn OnAlarmTurnedOn;

    public delegate void AlarmTurnedOff();
    public static event AlarmTurnedOff OnAlarmTurnedOff;


    public void SwitchAlarmOff(){
        //Debug.Log("Alarm system switched off");
        //FindObjectOfType<LevelManager>().SetIsAlarmSystemOn(false);
        if(OnAlarmTurnedOn != null){
            OnAlarmTurnedOff();
        }
    }

    public void SwitchAlarmOn(){
        //Debug.Log("Alarm system switched on");
        //FindObjectOfType<LevelManager>().SetIsAlarmSystemOn(true);
        if(OnAlarmTurnedOn != null){
            OnAlarmTurnedOn();
        }
        
    }
}
