using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitHandler : MonoBehaviour
{
    
    [SerializeField] LayerMask interactLayerMask;

    public void UseLevelExit(){
        LevelExit levelExit;
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, interactLayerMask);
        foreach (Collider coll in hitColliders){
            if(coll.gameObject.TryGetComponent<LevelExit>(out levelExit)){
                levelExit.ExitLevel();
            }
        }
    }

}
