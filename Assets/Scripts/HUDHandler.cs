using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUDHandler : MonoBehaviour
{

    [Header("UI text objects:")]
    [SerializeField] TextMeshProUGUI keysText;
    [SerializeField] TextMeshProUGUI lockpickText;

    [Header("UI Text starts")]
    [SerializeField] string keysTextStart;
    [SerializeField] string lockPickTextStart;

    private string keysString = "";
    
    // Start is called before the first frame update
    void Start()
    {
        SetLockPickCount(FindObjectOfType<KeyHandler>().GetLockPickCount());
        Debug.Log("Setting lock pick count to " + FindObjectOfType<KeyHandler>().GetLockPickCount() );
        SetCollectedKeys(FindObjectOfType<KeyHandler>().GetKeyString());
        Debug.Log("Setting key string to " + FindObjectOfType<KeyHandler>().GetKeyString());
    }


    public void SetLockPickCount(int lockPickCount)
    {
        lockpickText.text = lockPickTextStart + lockPickCount.ToString();
    }

    public void SetCollectedKeys(string keyName){
        if(keysString.Length >= 1){
            keysString = String.Concat(keysString, ", ", keyName);
        } else {
            keysString = keyName;
        }
        
        keysText.text = keysTextStart + keysString;

    }
}
