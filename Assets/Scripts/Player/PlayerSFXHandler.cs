using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXHandler : MonoBehaviour
{
    [SerializeField] AudioSource playerSFX;
    public AudioClip playerHeartBeatSFX;

    private void OnEnable() {
        LevelManager.OnGameOver += HandleGameOver;
    }

    private void OnDisable() {
        LevelManager.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        StopCurrentPlayerSFX();
    }

    public void PlayPlayerHeartBeat(){
        playerSFX.clip = playerHeartBeatSFX;
        playerSFX.Play();
    }

    public void StopCurrentPlayerSFX(){
        playerSFX.Stop();
    }

}
