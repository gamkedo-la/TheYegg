using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudioHandler : MonoBehaviour
{
    [Header("Audio clips for levels:")]
    public AudioClip[] audioClips;

    [Header("Required components")]
    [SerializeField] AudioSource levelAudioSource;

    public void PlayAudioForLevel(int index){
        if(audioClips.Length == 0)
        {
            Debug.LogWarning("No audioclips available for level audio handler!");
        } else {
            levelAudioSource.Stop();
            levelAudioSource.clip = audioClips[index];
            levelAudioSource.Play();
        }
        
    }

    public void StopLevelBackgroundMusic(){
        levelAudioSource.Stop();
    }
}
