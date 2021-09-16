using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    //this class handles the win and lose conditions of each level
    //attach a DontDestroy component to keep this persistent

    [Header("Scene settings")]
    [SerializeField] int currentLevelIndex;

 
    [SerializeField] int nextLevelIndex;
    [SerializeField] int mainMenuSceneIndex;
    [Tooltip("Set the level starting scenes' indeces for all the possible levels")]
    [SerializeField] List<int> levelStartIndeces = new List<int>();
    [SerializeField] List<Vector3> levelStartPositions = new List<Vector3>();


    [Header("UI Gameobjects")]
    [SerializeField] GameObject loseUI;
    [SerializeField] GameObject winUI;

    [Header("Scene fade parameters")]
    [SerializeField] Animator animator;

    private bool areCamerasDisabled;
    private int levelToLoad;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public bool GetCamerasDisabled()
    {
        return areCamerasDisabled;
    }

    public void SetCamerasDisabled(bool t)
    {
        areCamerasDisabled = t;
    }


    private void Start() {
        if(winUI){
            winUI.SetActive(false);
        }
        
        if(loseUI){
            loseUI.SetActive(false);
        }
    }


    public void LevelCleared(){
        //display score
        //TODO: ScoreKeeper.CalculateScore();
        //show option UI to move to next level
        winUI.SetActive(true);
        HUDHandler hUDHandler = FindObjectOfType<HUDHandler>();
        if(!hUDHandler || hUDHandler == null){
            Debug.LogWarning("LevelManager cannot find a HUDHandler");
        }
        hUDHandler.ToggleHUDVisibility(false); 
    }

    public void GameOver(){
        //show option UI to reset to level
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
        Debug.Log("Loading scene " + sceneIndex);
        //SceneManager.LoadScene(sceneIndex);
        FadeToLevel(sceneIndex);
        ChangeLevelIndeces();
        winUI.SetActive(false);
        //reset player parameters for lockpicks, keys, disguise and position in the game
        ResetPlayer();
    }

    private void ResetPlayer()
    {
        if(GameObject.FindObjectOfType<PlayerMovement>()){
            GameObject.FindObjectOfType<KeyHandler>().ResetKeyHandler();
            GameObject.FindObjectOfType<DisguiseHandler>().ResetDisguiseHandler();
        }
        areCamerasDisabled = false;
    }

    public void ReloadCurrentLevel(){
        int sceneIndex = levelStartIndeces[currentLevelIndex];
        Debug.Log("Loading scene " + sceneIndex);
        //SceneManager.LoadScene(sceneIndex);
        FadeToLevel(sceneIndex);
        winUI.SetActive(false);
        loseUI.SetActive(false);
        ResetPlayer();
        //reset player parameters for lockpicks, keys and disguise
    }

    public void LoadMainMenu(){
        Debug.Log("Loading scene " + mainMenuSceneIndex);
        currentLevelIndex = 0;
        nextLevelIndex = 0;
        winUI.SetActive(false);
        loseUI.SetActive(false);
        //SceneManager.LoadScene(mainMenuSceneIndex);
        FadeToLevel(mainMenuSceneIndex);
    }

    public void FadeToLevel(int levelIndex){
        animator.SetTrigger("FadeOut");
        levelToLoad = levelIndex;
    }

    public void OnFadeComplete(){
        SceneManager.LoadScene(levelToLoad);
        if(PlayerMovement.GetPlayer()){
            PlayerMovement.GetPlayer().transform.position = levelStartPositions[currentLevelIndex]; //NB main menu scene does not have a starting position
            //TODO make sure that when restarting from the first level the currentLevelIndex is reset correctly
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        animator.SetTrigger("FadeIn");
    }

}
