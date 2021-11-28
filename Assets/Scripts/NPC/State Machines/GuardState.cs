using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuardState : MonoBehaviour
{

    public GuardFSM guardFSM;
    public AudioClip[] enterStateSayings;

    public virtual void RunGuardState()
    {
        GuardStateCheck();
    }

    public virtual void StartGuardState()
    {
        if(enterStateSayings.Length>0) {
            AudioSource.PlayClipAtPoint(enterStateSayings[UnityEngine.Random.Range(0, enterStateSayings.Length)], transform.position);
        }
    }

    public virtual void EndGuardState(){
        guardFSM.PopState(this);
        //protect against having no state
        if(guardFSM.GetCurrentState() == null){
            guardFSM.PushState(guardFSM.defaultState);
        }
    }

    public virtual void GuardStateCheck()
    {
        //check for one common state transition: Any state -> Incapacitated
        //other checks inside the states themselves
    }

}
