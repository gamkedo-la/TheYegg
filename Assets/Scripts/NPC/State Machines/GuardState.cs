using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuardState : MonoBehaviour
{

    public GuardFSM guardFSM;
    public AudioClip[] enterStateSayings;
    public AudioClip[] exitStateSayings;
    private AudioClip playAfterStaggeredDelay; // uses to queue up start/end sayings so they won't all be in exact sync between guards

    private void Start() {
        StartCoroutine(PlayDelayedVoiceIfQueued());
    }

    IEnumerator PlayDelayedVoiceIfQueued() {
        while(true) {
            if(playAfterStaggeredDelay) {
                AudioSource.PlayClipAtPoint(playAfterStaggeredDelay, transform.position);
                // Debug.Log("Playing delayed transition voice sound");
                playAfterStaggeredDelay = null;
            }
            // randomized update interval to stagger when guards say their phrases, up to a few seconds later than each other
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.25f,3.0f));
        }
    }

    public virtual void RunGuardState()
    {
        GuardStateCheck();
    }

    public virtual void StartGuardState()
    {
        if(enterStateSayings.Length>0) {
            playAfterStaggeredDelay = enterStateSayings[UnityEngine.Random.Range(0, enterStateSayings.Length)];
        }
    }

    public virtual void EndGuardState(){
        if (exitStateSayings.Length > 0) {
            playAfterStaggeredDelay = exitStateSayings[UnityEngine.Random.Range(0, exitStateSayings.Length)];
        }
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
