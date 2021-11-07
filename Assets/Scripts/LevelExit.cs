using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    //the level exit represents the final piece of the level the player needs to reach in order to move on to the next level

    [SerializeField] GameObject levelExit; //a child object which visually represents the exit
    private LevelManager levelManager;
    private Collider exitCollider;

    private void Start() {
        exitCollider = GetComponent<Collider>();
        if(!exitCollider || exitCollider == null){
            Debug.LogWarning("Not collider found in LevelExit for " + gameObject.name);
        }
        levelManager = FindObjectOfType<LevelManager>();
        if(!levelManager || levelManager == null){
            Debug.LogWarning("No levelmanager found in LevelExit for " + gameObject.name);
        } else {
            bool isExitEnabled = levelManager.GetIsExitEnabled();
            SetExitActive(isExitEnabled);
        }
    }

    public void ExitLevel(){
        levelManager.LevelCompleted();
    }

    public void SetExitActive(bool t)
    {
        levelExit.SetActive(t);
        exitCollider.enabled = t;
    }
}
