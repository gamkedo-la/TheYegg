using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class HUDHandler : MonoBehaviour
{

    [Header("UI text objects:")]
    [SerializeField] TextMeshProUGUI keysText;
    [SerializeField] TextMeshProUGUI lockpickText;
    [SerializeField] TextMeshProUGUI disguiseStatusText;

    [Header("UI Text starts")]
    [SerializeField] string keysTextStart;
    [SerializeField] string lockPickTextStart;

    [Header("Disguise status texts")]
    [SerializeField] string disguiseCompromisedText;
    [SerializeField] string disguiseSafeText;

    private string keysString = "";
    
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void Start()
    {
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetLockPickCount(FindObjectOfType<KeyHandler>().GetLockPickCount());
        SetCollectedKeys(FindObjectOfType<KeyHandler>().GetKeyString());
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

    public void SetDisguiseStatus(bool isCompromised){
        if(isCompromised){
            disguiseStatusText.text = disguiseCompromisedText;
        } else {
            disguiseStatusText.text = disguiseSafeText;
        }
    }

    public void ToggleHUDVisibility(bool t){
        if(t == false){
            //make HUD objects invisible
            keysText.alpha = 0f;
            lockpickText.alpha = 0f;
            disguiseStatusText.alpha = 0f;
        } else {
            keysText.alpha = 255f;
            lockpickText.alpha = 255f;
            disguiseStatusText.alpha = 0f;
        }
    }
}
