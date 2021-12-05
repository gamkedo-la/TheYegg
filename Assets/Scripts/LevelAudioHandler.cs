using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudioHandler : MonoBehaviour
{
    [Header("Audio clips for levels:")]
    public AudioClip[] audioClips;

    [Header("Sound to start level :")] // ex. elevator ding
    public AudioClip[] audioSoundForStart;

    [Header("Background noises by level:")]
    public AudioClip[] randomEffectsJail;
    public AudioClip[] randomEffectsIndustrial;
    public AudioClip[] randomEffectsOffice;
    public AudioClip[] randomEffectsCasino;

    [Header("Required components")]
    [SerializeField] AudioSource levelAudioSource;

    private int currentLevelIndex = -1;

    public void PlayAudioForLevel(int index){
        currentLevelIndex = index;
        if (audioSoundForStart.Length>0 && audioSoundForStart[index]) {
            GameObject playerGO = GameObject.FindWithTag("Player");
            AudioSource.PlayClipAtPoint(audioSoundForStart[index], playerGO.transform.position);
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

    private void Start() {
        StartCoroutine(RandomBackgroundNoise());
    }

    IEnumerator RandomBackgroundNoise() {
        AudioClip[] soundList;
        while (true) {
            soundList = null;
            switch (currentLevelIndex) {
                case 0:
                    soundList = randomEffectsJail;
                    break;
                case 1:
                    soundList = randomEffectsIndustrial;
                    break;
                case 2:
                    soundList = randomEffectsOffice;
                    break;
                case 3:
                    soundList = randomEffectsCasino;
                    break;
                default:
                    soundList = null;
                    break;
            }
            if(soundList != null && soundList.Length>0) {
                AudioSource.PlayClipAtPoint(soundList[UnityEngine.Random.Range(0, soundList.Length)], Camera.main.transform.position);
            }
            yield return new WaitForSeconds(Random.Range(6.0f,17.0f)); // frequency of random environment audio
        }
    }

    public void StopLevelBackgroundMusic(){
        levelAudioSource.Stop();
    }
}
