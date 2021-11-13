using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSequenceHandler : MonoBehaviour
{
    //attach this component to Player to handle level sequences
    [Header("Interaction Settings")]
    [SerializeField] LayerMask interactLayerMask;


    private float timeToInteract;
    private LevelSequenceObject levelSequenceObject;

    public bool StartInteraction()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach(Collider hit in hitColliders){
            if(hit.TryGetComponent<LevelSequenceObject>(out levelSequenceObject)){
                timeToInteract = levelSequenceObject.GetTimeToInteract();
                return true;
            } else {
                levelSequenceObject = null;
            }
        } 

        return false;
    }

    public void InterAct(float inputTime){
        if(inputTime > timeToInteract){
            if(levelSequenceObject){
                levelSequenceObject.InteractWithObject();
            }
        }
    }

    public void HandleTimer(float v)
    {
        if(v >= timeToInteract){
            levelSequenceObject.SetTimerValue(1f);
        } else {
            levelSequenceObject.SetTimerValue(v / timeToInteract);
        }
    }
}


