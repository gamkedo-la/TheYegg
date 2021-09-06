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
        SceneManager.LoadScene(sceneIndex);
        if(PlayerMovement.GetPlayer()){
            PlayerMovement.GetPlayer().transform.position = levelStartPositions[0];
        }
        ChangeLevelIndeces();
        winUI.SetActive(false);
        //reset player parameters for lockpicks, keys, disguise and position in the game
    }

    public void ReloadCurrentLevel(){
        int sceneIndex = levelStartIndeces[currentLevelIndex];
        Debug.Log("Loading scene " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
        if(PlayerMovement.GetPlayer()){
            PlayerMovement.GetPlayer().transform.position = levelStartPositions[0];
        }
        winUI.SetActive(false);
        loseUI.SetActive(false);
        //reset player parameters for lockpicks, keys and disguise
    }

    public void LoadMainMenu(){
        Debug.Log("Loading scene " + mainMenuSceneIndex);
        SceneManager.LoadScene(mainMenuSceneIndex);
        currentLevelIndex = 0;
        nextLevelIndex = 0;
        winUI.SetActive(false);
        loseUI.SetActive(false);
    }

}
