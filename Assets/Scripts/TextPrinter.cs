using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPrinter : MonoBehaviour
{
    [Multiline]
    public List<string> textsToPrint;
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

    private IEnumerator PrintText()
    {
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
        yield return new WaitForSeconds(delayToTextDisappear);
        StartCoroutine(ClearText());

    }

    public void PrintlevelIntro(int index)
    {
        levelIndex = index;
        processedArray = textsToPrint[levelIndex].ToCharArray();
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




