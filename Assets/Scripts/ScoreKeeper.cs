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

    private float levelTotalTime;
    private float levelStartTime;

    private int timesDetected;
    private int numberOfSubduedGuards;
    private int numberOfDisguisesUsed;

    private int detectionPoints;
    private int subduePoints;
    private int disguisePoints;
    private int timePoints;
    private int totalScore;


    public void StartLevelTimer(){
        //level timer needs to be restarted when a new level is loaded by LevelManager
        levelStartTime = Time.time;
    }


    private void ResetScoreKeeper(){
        //reset all scores here other than level timer
        timesDetected = 0;
        numberOfDisguisesUsed = 0;
        numberOfSubduedGuards = 0;
    }

    //public methods for adding to score variables and getting score variables
    public void IncreaseTimesDetected(){
        timesDetected += 1;
    }

    public int GetTimesDetectedScore(){
        detectionPoints = timesDetected * subtractFromTimesDetected;
        return detectionPoints;
    }

    public void IncreaseDisguisesUsed(){
        numberOfDisguisesUsed += 1;
    }

    public int GetDisguiseScore(){
        disguisePoints = numberOfDisguisesUsed * subtractFromDisguisesUsed;
        return disguisePoints;
    }
    
    public void IncreaseGuardsSubdued(){
        numberOfSubduedGuards += 1;
    }

    public int GetSubdueScore(){
        subduePoints  = numberOfSubduedGuards * subtractFromSubduedGuard;
        return subduePoints;
    }

    public int GetTimeScore(){
        levelTotalTime = Time.time - levelStartTime;
        timePoints = Mathf.RoundToInt(levelTotalTime) * subtractFromSeconds;
        return timePoints;
    }
    
    public int GetTotalScore(){
        totalScore = maximumPoints - detectionPoints - subduePoints - disguisePoints - timePoints;
        return totalScore;
    }
}
