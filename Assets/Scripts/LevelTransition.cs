using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelTransition : MonoBehaviour
{

    [SerializeField] int toLevelIndex;

    private Camera mainCamera;
    private Vector2 currentVelocity;


    public void TransitionToLevel(){
        SceneManager.LoadScene(toLevelIndex);
    }


}
