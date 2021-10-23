using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    
    //contains all the required public methods to control volume in the in game menu

    public AudioMixer audioMixer;
    [SerializeField] InGameMenuHandler inGameMenuHandler;
    [SerializeField] KeyCode muteSound;

    private bool isMuted = false;

    private void Start() {
        inGameMenuHandler = FindObjectOfType<InGameMenuHandler>();
        if(!inGameMenuHandler || inGameMenuHandler == null){
            Debug.Log("No in game menu handler found in Audio Controller!");
        }
    }

    public void SetMasterVolume(Slider volume){
        audioMixer.SetFloat("masterVolume", volume.value / 100f * -80f);
        inGameMenuHandler.SetMasterVolumeValue(100 - Mathf.RoundToInt(volume.value));
    }

    public void SetSFXVolume(Slider volume){
        audioMixer.SetFloat("sfxVolume", volume.value / 100f * -80f);
        inGameMenuHandler.SetSFXVolumeValue(100 - Mathf.RoundToInt(volume.value));
    }

    public void SetMusicVolume(Slider volume){
        audioMixer.SetFloat("musicVolume", volume.value / 100f * -80f);
        inGameMenuHandler.SetBGVolumeValue(100 - Mathf.RoundToInt(volume.value));
    }

    private void Update()
    {
        if (Input.GetKeyDown(muteSound))
        {
            isMuted = !isMuted;
            AudioListener.volume = isMuted ? 0 : 1;
        }
    }

}
