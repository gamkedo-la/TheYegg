using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPrinter : MonoBehaviour
{
    [Multiline]
    public string textToPrint;
    private char[] processedArray;
    private char[] textToPrintSplit;
    public TextMeshProUGUI textPrinterText;
    public float delayBetweenCharacters;
    private bool isPrinting = false;


    private void Start() {
        processedArray = textToPrint.ToCharArray();
        textToPrintSplit = new char[processedArray.Length]; 
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isPrinting)
            {
                textToPrintSplit = new char[processedArray.Length]; 
                StartCoroutine(PrintText());
            }
        }
        
    }

    private IEnumerator PrintText()
    {
        isPrinting = true;
        for (int i = 0; i < processedArray.Length; i++)
        {
            textToPrintSplit[i] = processedArray[i];
            string s = new string(textToPrintSplit);
            textPrinterText.text = s;
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
        isPrinting = false;
    }

}
