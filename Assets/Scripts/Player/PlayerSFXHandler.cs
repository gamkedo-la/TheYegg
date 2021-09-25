using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXHandler : MonoBehaviour
{
    [SerializeField] AudioSource playerSFX;
    public AudioClip playerHeartBeatSFX;

    public void PlayPlayerHeartBeat(){
        playerSFX.clip = playerHeartBeatSFX;
        playerSFX.Play();
    }

    public void StopCurrentPlayerSFX(){
        playerSFX.Stop();
    }

}
