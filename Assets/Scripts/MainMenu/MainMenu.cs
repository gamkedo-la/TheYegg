using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void startGame()
    {
        //SceneManager.LoadScene(startScenePath, LoadSceneMode.Single);
        FindObjectOfType<LevelManager>().LoadNextLevel();
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
