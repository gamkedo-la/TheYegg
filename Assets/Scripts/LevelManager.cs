using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    //this class handles the win and lose conditions of each level
    //attach a DontDestroy component to keep this persistent


    //event telling that the game is over
    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public delegate void LevelCleared();
    public static event LevelCleared OnLevelCleared;

    [Header("Scene settings")]
    [SerializeField] int currentLevelIndex;

 
    [SerializeField] int nextLevelIndex;
    [SerializeField] int mainMenuSceneIndex;
    [Tooltip("Set the level starting scenes' indeces for all the possible levels")]
    [SerializeField] List<int> levelStartIndeces = new List<int>();
    [SerializeField] List<Vector3> levelStartPositions = new List<Vector3>();

    [Header("Number of conditions to check for clearing level")]
    [SerializeField] List<int> levelClearConditionCount = new List<int>();


    [Header("UI Gameobjects")]
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject winUI;

    [Header("Scene fade parameters")]
    [SerializeField] Animator animator;

    private bool areCamerasDisabled;
    private int levelToLoad;
    private int currentLevelConditionsCleared = 0;
    private bool isExitEnabled = false;
    private ScoreKeeper scoreKeeper;
    private bool isAlarmOn = false;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        AlarmSystemSwitch.OnAlarmTurnedOff += HandleAlarmTurnedOff;
        AlarmSystemSwitch.OnAlarmTurnedOn += HandleAlarmTurnedOn;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
        AlarmSystemSwitch.OnAlarmTurnedOff -= HandleAlarmTurnedOff;
        AlarmSystemSwitch.OnAlarmTurnedOn -= HandleAlarmTurnedOn;
    }

    private void Start() {
        if(winUI){
            winUI.SetActive(false);
        }
        
        if(loseUI){
            loseUI.SetActive(false);
        }
    }

    public bool GetCamerasDisabled()
    {
        return areCamerasDisabled;
    }

    public void SetCamerasDisabled(bool t)
    {
        areCamerasDisabled = t;
    }


    public bool GetIsExitEnabled()
    {
        return isExitEnabled;
    }

    public void SetIsAlarmSystemOn(bool t){
        isAlarmOn = t;
    }

    public bool GetIsAlarmSystemOn(){
        return isAlarmOn;
    }

    public void LevelClearConditionCompleted(int index){
        //increase the number of conditions cleared
        currentLevelConditionsCleared = index;
        if(currentLevelConditionsCleared >= levelClearConditionCount[currentLevelIndex]){
            isExitEnabled = true;
            //TODO enable all exits in the same scene
        }
    }

    public int GetLevelClearConditionCompleted(){
        return currentLevelConditionsCleared;
    }


    public void LevelCompleted(){
        //trigger event that level is cleared
        OnLevelCleared();
        //display score
        //show option UI to move to next level
        winUI.SetActive(true);
        winUI.GetComponent<ScoreUIHandler>().SetScoreTexts();
        //TODO move this to HUDHandler triggered by event
        HUDHandler hUDHandler = FindObjectOfType<HUDHandler>();
        if(!hUDHandler || hUDHandler == null){
            Debug.LogWarning("LevelManager cannot find a HUDHandler");
        }
        hUDHandler.ToggleHUDVisibility(false); 
    }

    public void StartGameOver(){
        //set event that game is over
        OnGameOver();
        //show option UI to reset to level
        loseUI.SetActive(true);
        HUDHandler hUDHandler = FindObjectOfType<HUDHandler>();
        if(!hUDHandler || hUDHandler == null){
            Debug.LogWarning("LevelManager cannot find a HUDHandler");
        }
        hUDHandler.ToggleHUDVisibility(false); 
    }

    private void ChangeLevelIndeces(){
        currentLevelIndex += 1;
        if(levelStartIndeces.Count > nextLevelIndex){
            nextLevelIndex += 1;
        } else {
            nextLevelIndex = mainMenuSceneIndex;
        }
    }

    public void LoadNextLevel(){
        int sceneIndex = levelStartIndeces[nextLevelIndex];
        FadeToLevel(sceneIndex);
        ChangeLevelIndeces();
        winUI.SetActive(false);
        loseUI.SetActive(false);
        ResetPlayer();
        currentLevelConditionsCleared = 0;
        isExitEnabled = false;
    }

    private void ResetPlayer()
    {
        if(GameObject.FindObjectOfType<PlayerMovement>()){
            GameObject.FindObjectOfType<KeyHandler>().ResetKeyHandler();
            GameObject.FindObjectOfType<DisguiseHandler>().ResetDisguiseHandler();
            GameObject.FindObjectOfType<PlayerActionController>().SetIsInRestrictedArea(false);
            GameObject.FindObjectOfType<NPCHandler>().ResetNPCList();
        }
        areCamerasDisabled = false;
    }

    public void ReloadCurrentLevel(){
        int sceneIndex = levelStartIndeces[currentLevelIndex];
        FadeToLevel(sceneIndex);
        winUI.SetActive(false);
        loseUI.SetActive(false);
        ResetPlayer();
        currentLevelConditionsCleared = 0;
        isExitEnabled = false;
    }

    public void LoadMainMenu(){
        currentLevelIndex = -1;
        nextLevelIndex = 0;
        winUI.SetActive(false);
        loseUI.SetActive(false);
        FadeToLevel(mainMenuSceneIndex);
    }

    public void FadeToLevel(int levelIndex){
        animator.SetTrigger("FadeOut");
        levelToLoad = levelIndex;
    }

    public void OnFadeComplete(){
        SceneManager.LoadScene(levelToLoad);
        if(PlayerMovement.GetPlayer() && currentLevelIndex >= 0){
            PlayerMovement.GetPlayer().transform.position = levelStartPositions[currentLevelIndex]; //NB main menu scene does not have a starting position
            //TODO make sure that when restarting from the first level the currentLevelIndex is reset correctly
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        animator.SetTrigger("FadeIn");
    }

    private void HandleAlarmTurnedOn()
    {
        SetIsAlarmSystemOn(true);
    }

    private void HandleAlarmTurnedOff()
    {
        SetIsAlarmSystemOn(false);
    }

}
