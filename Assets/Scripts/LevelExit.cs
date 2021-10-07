using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    //the level exit represents the final piece of the level the player needs to reach in order to move on to the next level

    [SerializeField] GameObject levelExit; //a child object which visually represents the exit
    private LevelManager levelManager;


    private void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        if(!levelManager || levelManager == null){
            Debug.LogWarning("No levelmanager found in LevelExit for GO " + gameObject.name);
        } else {
            levelExit.SetActive(levelManager.GetIsExitEnabled());
        }
    }

    public void ExitLevel(){
        levelManager.LevelCompleted();
    }

}
