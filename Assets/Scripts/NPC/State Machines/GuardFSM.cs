using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFSM : MonoBehaviour
{
    public GuardState activeState;
    public GuardState defaultState;
    public GuardState patrolState;
    public GuardState alertState;
    public GuardState idleState;
    public GuardState incapacitatedState;
    public List<GuardState> stateStack;
    [HideInInspector]
    public GuardFSM guardFSM;

    private void Start() {
        stateStack = new List<GuardState>();
        guardFSM = this;
        PushState(defaultState);
    }

    public void Update() {
        activeState = GetCurrentState();
        if(activeState != null){
            activeState.RunGuardState();
        }
    }

    public void PopState(GuardState state){
        stateStack.Remove(state);
    }

    public void PushState(GuardState state){
        //protect against duplicates of the same state in the stack
        if(GetCurrentState() != state){
            stateStack.Insert(0, state);
            stateStack[0].StartGuardState();
        }             
        
    }

    public GuardState GetCurrentState()
    {
        if(stateStack.Count > 0){
            return stateStack[stateStack.Count - 1];
        } else {
            return null;
        }
    }

}
