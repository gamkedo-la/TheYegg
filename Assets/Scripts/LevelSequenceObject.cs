using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSequenceObject : MonoBehaviour
{
    [Header("Interaction settings")]
    [SerializeField] float timeToInteract;
    [Tooltip("What is the position of this object in the correct sequence of clearing the level")]
    [SerializeField] int positionInSequence;

    public void InteractWithObject(){
        Debug.Log("Sequence step completed!");
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if(levelManager){
            //if has cleared previous condition, set this condition to be cleared
            if(levelManager.GetLevelClearConditionCompleted() == positionInSequence - 1){
                levelManager.LevelClearConditionCompleted(positionInSequence);
            }
        }
    }

    public float GetTimeToInteract()
    {
        return timeToInteract;
    }

}
