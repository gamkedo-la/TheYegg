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

    [SerializeField] string totalScoreStringStart = "Total Score: ";
    [SerializeField] string timeToClearStringStart = "Time To Clear: ";
    [SerializeField] string guardsSubduedStringStart = "Guards Subdued: ";
    [SerializeField] string disguisesUsedStringStart = "Disguises Used: ";
    [SerializeField] string timesDetectedStringStart = "Times Detected: ";
    

    private ScoreKeeper scoreKeeper;
    
    public void SetScoreTexts(){
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(!scoreKeeper || scoreKeeper == null){
            Debug.LogWarning("No ScoreKeeper found in scene for " + gameObject.name);
        }
        int totalScore = scoreKeeper.GetTotalScore();
        float timeToClear = scoreKeeper.GetTimeToClear();
        float minutes = Mathf.Floor(timeToClear / 60);
        float seconds = Mathf.RoundToInt(timeToClear%60);
        string timeString = minutes.ToString() + ":" + seconds.ToString();
        int guardsSubdued = scoreKeeper.GetSubduedCount();
        int disguisesUsed = scoreKeeper.GetDisguisesUsedCount();
        int timesDetected = scoreKeeper.GetTimesDetected();
        
        totalScoreText.text = totalScoreStringStart + totalScore.ToString();
        timeToClearText.text = timeToClearStringStart + timeString;
        guardsSubduedText.text = guardsSubduedStringStart + guardsSubdued.ToString();
        disguisesUsedText.text = disguisesUsedStringStart + disguisesUsed.ToString();
        timesDetectedText.text = timesDetectedStringStart + timesDetected.ToString();
        
    }
}
