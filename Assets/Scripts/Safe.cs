using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{

    [Header("Safe settings")]
    [SerializeField] float timeToOpen;
    [Tooltip("What is the position of this object in the correct sequence of clearing the level")]
    [SerializeField] int positionInSequence;

    public void OpenSafe(){
        Debug.Log("Safe opened!");
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if(levelManager){
            //if has cleared previous condition, set this condition to be cleared
            if(levelManager.GetLevelClearConditionCompleted() == positionInSequence - 1){
                levelManager.LevelClearConditionCompleted(positionInSequence);
            }
        }
    }

    public float GetTimeToOpen()
    {
        return timeToOpen;
    }

}
