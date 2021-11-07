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
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if(levelManager){
            Debug.Log("Found levelManager!");
            //if has cleared previous condition, set this condition to be cleared
            if(levelManager.GetLevelClearConditionCompleted() == positionInSequence - 1){
                Debug.Log("Cleared sequence step!");
                levelManager.LevelClearConditionCompleted(positionInSequence);
            }
        }
    }

    public float GetTimeToInteract()
    {
        return timeToInteract;
    }

}
