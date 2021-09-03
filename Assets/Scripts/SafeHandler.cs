using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeHandler : MonoBehaviour
{

    [Header("Safe Interaction Settings")]
    [SerializeField] LayerMask interactLayerMask;


    private float timeToOpenSafe;
    private Safe safe;

    public bool StartOpenSafe()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach(Collider hit in hitColliders){
            if(hit.TryGetComponent<Safe>(out safe)){
                timeToOpenSafe = safe.GetTimeToOpen();
                return true;
            }
        }

        return false;
    }

    public void OpenSafe(float inputTime){
        if(inputTime > timeToOpenSafe){
            safe.OpenSafe();
        }
    }
}
