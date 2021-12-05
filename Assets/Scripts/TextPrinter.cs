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
    private bool isProcessed = false;
    [SerializeField] KeyCode continueTextInput;
    [SerializeField] string waitForInputPrompt;

    private void ProcessTexts() {
        //move the inserted strings to list of list 
        //0 and 1 under 0 in list of lists
        /*
        textsToPrintInChunks[0].Insert(0, textsToPrint[0]);
        Debug.Log("Added 00");
        textsToPrintInChunks[0].Insert(1, textsToPrint[1]);
        textsToPrintInChunks[1].Insert(0, textsToPrint[2]);
        textsToPrintInChunks[1].Insert(1, textsToPrint[3]);
        textsToPrintInChunks[2].Insert(0, textsToPrint[4]);
        textsToPrintInChunks[2].Insert(1, textsToPrint[5]);
        textsToPrintInChunks[3].Insert(0, textsToPrint[6]);
        textsToPrintInChunks[3].Insert(1, textsToPrint[7]);
        */
        isProcessed = true;
    }

    private void Update() {
        if(Input.GetKeyDown(continueTextInput) && waitForInput)
        {
            waitForInput = false;
            //continue to next chunk
            chunkIndex++;
            if(chunkIndex > textsToPrint.levelIntroTexts[levelIndex].Count() - 1)
            {
                Debug.Log("Showing mission objectives");
                //if no more chunks to print, show level objectives instead
                ShowMissionObjectives();
            }
            else
            {   
                Debug.Log("Printing next chunk");
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
        Debug.Log("Waiting for input");
        string cont = new string(textToPrintSplit) + waitForInputPrompt;
        ///yield return new WaitForSeconds(delayToTextDisappear);
        textPrinterText.text = cont;
        //StartCoroutine(ClearText());

    }

    public void PrintlevelIntro(int index)
    {
        Debug.Log("Calling PrintLevelIntro with index " + index);
        if(!isProcessed)
        {
            ProcessTexts();
            Debug.Log("Processed texts. Chunk count is " + textsToPrint.levelIntroTexts.Count );
            Debug.Log("Processed texts. Chunk count in 0 is " + textsToPrint.levelIntroTexts[0].Count() );
        }
        levelIndex = index;
        Debug.Log("Accessing text array with indexes " + levelIndex + " " + chunkIndex);
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




