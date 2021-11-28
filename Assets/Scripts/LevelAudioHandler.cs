using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudioHandler : MonoBehaviour
{
    [Header("Audio clips for levels:")]
    public AudioClip[] audioClips;

    [Header("Sound to start level :")] // ex. elevator ding
    public AudioClip[] audioSoundForStart;

    [Header("Required components")]
    [SerializeField] AudioSource levelAudioSource;

    public void PlayAudioForLevel(int index){
        Debug.Log("PlayAudioForLevel");
        if(audioSoundForStart.Length>0 && audioSoundForStart[index]) {
            AudioSource.PlayClipAtPoint(audioSoundForStart[index], Camera.main.transform.position);
        }
        if (audioClips.Length == 0)
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
