using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    
    //Scorekeeper keeps score of events in each level which will increase/decrease the player's score
    //attach a DontDestroy to keep this object persistent in the scene
    [SerializeField] int maximumPoints;
    [Header("Point settings for game events")]
    [SerializeField] int subtractFromTimesDetected;
    [SerializeField] int subtractFromSubduedGuard;
    [SerializeField] int subtractFromDisguisesUsed;
    [SerializeField] int subtractFromSeconds;
    [SerializeField] int subtractFromSpottedByCameras;

    private float levelTotalTime;
    private float levelStartTime;

    public int timesDetected;
    public int numberOfSubduedGuards;
    public int numberOfDisguisesUsed;

    private int detectionPoints;
    private int subduePoints;
    private int disguisePoints;
    private int timePoints;
    private int cameraPoints;
    private int totalScore;
    public bool isDetectedByCameras;


    private void OnEnable() {
        LevelManager.OnLevelCleared += HandleLevelCleared;
    }

    private void OnDisable() {
        LevelManager.OnLevelCleared -= HandleLevelCleared;
    }

    private void HandleLevelCleared(){
        StopLevelTimer();
    }

    public void StartLevelTimer(){
        //TODO level timer needs to be restarted when a new level is loaded by LevelManager
        levelStartTime = Time.time;
    }

    public void StopLevelTimer(){
        levelTotalTime = Time.time - levelStartTime;
    }


    private void ResetScoreKeeper(){
        //reset all scores here other than level timer
        timesDetected = 0;
        numberOfDisguisesUsed = 0;
        numberOfSubduedGuards = 0;
        isDetectedByCameras = false;
    }

    //public methods for adding to score variables and getting score variables
    public void IncreaseTimesDetected(){
        timesDetected += 1;
    }

    public int GetTimesDetected(){
        return timesDetected;
    }

    public int GetTimesDetectedScore(){
        detectionPoints = timesDetected * subtractFromTimesDetected;
        return detectionPoints;
    }

    public void IncreaseDisguisesUsed(){
        numberOfDisguisesUsed += 1;
    }

    public int GetDisguisesUsedCount(){
        return numberOfDisguisesUsed;
    }

    public int GetDisguiseScore(){
        disguisePoints = numberOfDisguisesUsed * subtractFromDisguisesUsed;
        return disguisePoints;
    }
    
    public void IncreaseGuardsSubdued(){
        numberOfSubduedGuards += 1;
    }

    public int GetSubduedCount(){
        return numberOfSubduedGuards;
    }

    public int GetSubdueScore(){
        subduePoints  = numberOfSubduedGuards * subtractFromSubduedGuard;
        return subduePoints;
    }

    public int GetTimeScore(){
        timePoints = Mathf.RoundToInt(levelTotalTime) * subtractFromSeconds;
        return timePoints;
    }

    public float GetTimeToClear(){
        return levelTotalTime;
    }

    public int GetCameraScore(){
        cameraPoints = subtractFromSpottedByCameras * (isDetectedByCameras ? 1 : 0);
        return cameraPoints;
    }
    
    public int GetTotalScore(){
        GetTimesDetectedScore();
        GetDisguiseScore();
        GetSubdueScore();
        GetTimeScore();
        GetCameraScore();
        totalScore = maximumPoints - detectionPoints - subduePoints - disguisePoints - timePoints - cameraPoints;
        return totalScore;
    }

    public void SetIsDetectedByCameras(bool t){
        isDetectedByCameras = t;
    }

    public bool GetIsDetectedByCameras(){
        return isDetectedByCameras;
    }
}
