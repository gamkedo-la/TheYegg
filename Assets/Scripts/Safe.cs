using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{

    [Header("Safe settings")]
    [SerializeField] float timeToOpen;

    public void OpenSafe(){
        Debug.Log("Safe opened!");
        FindObjectOfType<LevelManager>().LevelCleared();

    }

    public float GetTimeToOpen()
    {
        return timeToOpen;
    }

}
