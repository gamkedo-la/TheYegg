using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("Required Starting Scene")]
    [SerializeField] public string startScenePath;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
