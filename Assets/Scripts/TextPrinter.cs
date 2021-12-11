using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
public class TextsToPrint
{
    [Multiline]
    public List<string> textChunk = new List<string>(); //two chunks that are needed per level
    public int Count()
    {
        return textChunk.Count;
    }
}

[System.Serializable]
public class LevelIntroTextInChunks
{
    public List<TextsToPrint> levelIntroTexts = new List<TextsToPrint>();
}


public class TextPrinter : MonoBehaviour
{
    
    public LevelIntroTextInChunks textsToPrint = new LevelIntroTextInChunks();
    [Multiline]
    public List<string> missionObjectives;
    private char[] processedArray;
    private char[] textToPrintSplit;
    public TextMeshProUGUI textPrinterText;
    public float delayBetweenCharactersIncoming;
    public float delayBetweenCharactersRemoving;
    public float delayToTextDisappear = 5f;
    private bool isPrinting = false;
    private int levelIndex = 0;
    private int chunkIndex = 0;
    private bool waitForInput;
    [SerializeField] KeyCode continueTextInput;
    [SerializeField] string waitForInputPrompt;

    private void Start() {
        //if level is not changing, show mission objectives directly
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if(levelManager.isLevelChanging == false)
        {
            levelIndex = levelManager.GetCurrentLevelIndex();
            ShowMissionObjectives();
            
        }
    }

    private void Update() {
        if(Input.GetKeyDown(continueTextInput) && waitForInput)
        {
            waitForInput = false;
            //continue to next chunk
            chunkIndex++;
            if(chunkIndex > textsToPrint.levelIntroTexts[levelIndex].Count() - 1)
            {
                //if no more chunks to print, show level objectives instead
                ShowMissionObjectives();
            }
            else
            {   
                processedArray = textsToPrint.levelIntroTexts[levelIndex].textChunk[chunkIndex].ToCharArray();
                textToPrintSplit = new char[processedArray.Length];
                if(!isPrinting)
                {
                    StartCoroutine(PrintText());
                }
                
            }
        }
    }


    private IEnumerator PrintText()
    {
        
        textPrinterText.text = string.Empty;
        textPrinterText.alignment = TextAlignmentOptions.TopLeft;
        isPrinting = true;
        for (int i = 0; i < processedArray.Length; i++)
        {
            textToPrintSplit[i] = processedArray[i];
            string s = new string(textToPrintSplit);
            textPrinterText.text = s;
            yield return new WaitForSeconds(delayBetweenCharactersIncoming);
        }
        isPrinting = false;
        waitForInput = true;
        string cont = new string(textToPrintSplit) + waitForInputPrompt;
        ///yield return new WaitForSeconds(delayToTextDisappear);
        textPrinterText.text = cont;
        //StartCoroutine(ClearText());

    }

    public void PrintlevelIntro(int index)
    {
        levelIndex = index;
        processedArray = textsToPrint.levelIntroTexts[levelIndex].textChunk[chunkIndex].ToCharArray();
        textToPrintSplit = new char[processedArray.Length]; 

        if(!isPrinting)
            {
                textToPrintSplit = new char[processedArray.Length]; 
                StartCoroutine(PrintText());
            }
    }

    private IEnumerator ClearText()
    {
        
        for(int i = processedArray.Length; i > 0; i--)
        {
            textToPrintSplit[i - 1] = new char();
            string s = new string(textToPrintSplit);
            textPrinterText.text = s;
            yield return new WaitForSeconds(delayBetweenCharactersRemoving);
        }
        ShowMissionObjectives();
    }

    private void ShowMissionObjectives()
    {
        textPrinterText.alignment = TextAlignmentOptions.BottomLeft;
        textPrinterText.text = missionObjectives[levelIndex];
    }


}




