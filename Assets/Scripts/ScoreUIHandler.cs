using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI timeToClearText;
    [SerializeField] TextMeshProUGUI guardsSubduedText;
    [SerializeField] TextMeshProUGUI disguisesUsedText;
    [SerializeField] TextMeshProUGUI timesDetectedText;
    [SerializeField] TextMeshProUGUI cameraDetectionText;

    [SerializeField] string totalScoreStringStart = "Total Score: ";
    [SerializeField] string timeToClearStringStart = "Time To Clear: ";
    [SerializeField] string guardsSubduedStringStart = "Guards Subdued: ";
    [SerializeField] string disguisesUsedStringStart = "Disguises Used: ";
    [SerializeField] string timesDetectedStringStart = "Times Detected: ";
    [SerializeField] string detectedByCamerasString = "Was Detected By Surveillance System";
    [SerializeField] string notDetectedByCamerasString = "Avoided or Disabled Surveillance System";
    

    private ScoreKeeper scoreKeeper;
    
    public void SetScoreTexts(){
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(!scoreKeeper || scoreKeeper == null){
            Debug.LogWarning("No ScoreKeeper found in scene for " + gameObject.name);
        }
        int totalScore = scoreKeeper.GetTotalScore();
        float timeToClear = scoreKeeper.GetTimeToClear();
        float minutes = Mathf.Floor(timeToClear / 60);
        float seconds = Mathf.RoundToInt(timeToClear % 60);
        string minutesString;
        if(minutes < 10) {
            minutesString = "0" + minutes.ToString();
        } else {
            minutesString = minutes.ToString();
        }
        string secondsString;
        if(seconds < 10) {
        secondsString = "0" + seconds.ToString();
        } else {
            secondsString = seconds.ToString();
        }
        
        string timeString = minutesString + ":" + secondsString;
        int guardsSubdued = scoreKeeper.GetSubduedCount();
        int disguisesUsed = scoreKeeper.GetDisguisesUsedCount();
        int timesDetected = scoreKeeper.GetTimesDetected();

        string cameraString;
        if(scoreKeeper.GetIsDetectedByCameras() == true){
            cameraString = detectedByCamerasString;
        } else {
            cameraString = notDetectedByCamerasString;
        }
        
        totalScoreText.text = totalScoreStringStart + totalScore.ToString();
        timeToClearText.text = timeToClearStringStart + timeString;
        guardsSubduedText.text = guardsSubduedStringStart + guardsSubdued.ToString();
        disguisesUsedText.text = disguisesUsedStringStart + disguisesUsed.ToString();
        timesDetectedText.text = timesDetectedStringStart + timesDetected.ToString();
        cameraDetectionText.text = cameraString;
    }
}
