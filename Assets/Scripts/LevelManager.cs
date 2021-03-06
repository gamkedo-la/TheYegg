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
    [SerializeField] GameObject gameEndUI;

    [Header("Scene fade parameters")]
    [SerializeField] Animator animator;

    [Header("Audio handling components")]
    [SerializeField] LevelAudioHandler levelAudioHandler;

    public List<string> openedDoors = new List<string>();
    private bool areCamerasDisabled;
    private int levelToLoad;
    public int currentLevelConditionsCleared = 0;
    private bool isExitEnabled = false;
    private ScoreKeeper scoreKeeper;
    private bool isAlarmOn = false;
    public bool isLevelChanging = false;

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
        if(gameEndUI){
            gameEndUI.SetActive(false);
        }

        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(!scoreKeeper || scoreKeeper == null){
            Debug.LogWarning("LevelManager was not able to find a ScoreKeeper in scene!");
        }
    }

    public bool GetIsDoorOpen(string id) {
        foreach (var d in openedDoors)
        {
            if(d == id && d != null) 
            {
                return true;
            }
        }
        return false;
    }

    public void AddToOpenedDoors(string id){
        if(!openedDoors.Contains(id))
        {
            openedDoors.Add(id);
        }
    }

    public void ResetOpenedDoors(){
        openedDoors.Clear();
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

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public void LevelClearConditionCompleted(int index){
        //increase the number of conditions cleared
        Debug.Log("Level conditions completed");
        currentLevelConditionsCleared = index;
        if(currentLevelConditionsCleared >= levelClearConditionCount[currentLevelIndex]){
            isExitEnabled = true;
            LevelExit[] levelExits = FindObjectsOfType<LevelExit>();
            foreach (LevelExit exit in levelExits)
            {
                exit.SetExitActive(true);
            }
            
        }
    }

    public int GetLevelClearConditionCompleted(){
        return currentLevelConditionsCleared;
    }


    public void LevelCompleted(){
        //trigger event that level is cleared
        OnLevelCleared();
        Time.timeScale = 0f;
        //display score
        //show option UI to move to next level
        if(currentLevelIndex < 3)
        {
            winUI.SetActive(true);
            winUI.GetComponent<ScoreUIHandler>().SetScoreTexts();
        }
        else 
        {
            gameEndUI.SetActive(true);
            gameEndUI.GetComponent<ScoreUIHandler>().SetScoreTexts();
        }
        
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
        Time.timeScale = 0f;
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
        gameEndUI.SetActive(false);
        loseUI.SetActive(false);
        ResetPlayer();
        SetIsAlarmSystemOn(false);
        ResetOpenedDoors();
        scoreKeeper.ResetScoreKeeper();
        scoreKeeper.StartLevelTimer();
        currentLevelConditionsCleared = 0;
        isExitEnabled = false;
        isLevelChanging = true;
        Time.timeScale = 1f;

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
        gameEndUI.SetActive(false);
        loseUI.SetActive(false);
        ResetPlayer();
        currentLevelConditionsCleared = 0;
        isExitEnabled = false;
        isLevelChanging = true;
        SetIsAlarmSystemOn(false);
        ResetOpenedDoors();
        scoreKeeper.ResetScoreKeeper();
        scoreKeeper.StartLevelTimer();
        Time.timeScale = 1f;
    }

    private void PrintLevelIntro()
    {
        TextPrinter textPrinter = FindObjectOfType<TextPrinter>();
        if(textPrinter)
        {
            textPrinter.PrintlevelIntro(currentLevelIndex);
        }

    }

    public void LoadMainMenu(){
        currentLevelIndex = -1;
        nextLevelIndex = 0;
        winUI.SetActive(false);
        gameEndUI.SetActive(false);
        loseUI.SetActive(false);
        FadeToLevel(mainMenuSceneIndex);
        levelAudioHandler.StopLevelBackgroundMusic();
        Time.timeScale = 1f;
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
        if(isLevelChanging == true)
        {
            //show intros and switch audio clip
            PrintLevelIntro();
            levelAudioHandler.PlayAudioForLevel(currentLevelIndex);
            isLevelChanging = false;
        }
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
