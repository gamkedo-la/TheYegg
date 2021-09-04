using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransitionHandler : MonoBehaviour
{
    
    [SerializeField] LayerMask interactLayerMask;

    public void UseLevelTransition(){
        LevelTransition levelTransition;
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach (Collider coll in hitColliders){
            if(coll.gameObject.TryGetComponent<LevelTransition>(out levelTransition)){
                levelTransition.TransitionToLevel();
            }
        }
    }

}
