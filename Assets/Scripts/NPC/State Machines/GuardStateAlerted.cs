using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateAlerted : GuardState
{
    
    public Vector3 lastKnownLocation;
    public bool isAlertedByAnother;
    [SerializeField] float alertRadius;
    [SerializeField] LayerMask alertLayerMask;

    public override void StartGuardState()
    {
        base.StartGuardState();
        //alert all guards inside alerting radius IF this guard is the first to detect something
        if(!isAlertedByAnother){
            GetGuardsToAlert();
        }
        
    }

    public override void RunGuardState(){
        base.RunGuardState();
        
    }

    public override void EndGuardState()
    {
        SetIsAlertedByAnother(false);
        SetLastKnownLocation(Vector3.zero);
        base.EndGuardState();
    }
        
    private void GetGuardsToAlert()
    {
        GuardFSM[] allGuards = FindObjectsOfType<GuardFSM>();
        foreach(GuardFSM g in allGuards){
            if(Vector3.Distance(g.transform.position, lastKnownLocation) <= alertRadius){
                //raycast to the guard in the radius to see if there is a wall in between
                Vector3 dirToGuard = g.transform.position - transform.position;
                Ray ray = new Ray(transform.position, (dirToGuard));
                if(Physics.Raycast(ray, alertRadius, alertLayerMask)){
                    //alert others
                    if(g.activeState != g.alertState){
                        g.gameObject.GetComponent<GuardStateAlerted>().SetIsAlertedByAnother(true);
                        g.PushState(g.alertState);
                        g.activeState.EndGuardState();
                    }
                    
                }
            }
        }
    }

    public void SetLastKnownLocation(Vector3 loc){
        lastKnownLocation = loc;
    }

    public void SetIsAlertedByAnother(bool t){
        isAlertedByAnother = t;
    }

}
