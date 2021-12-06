using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSFXPlayer : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    [SerializeField] float minDistanceToPlayer;
    [SerializeField] float maxDistanceToPlayer;
    public bool isPlayingAudio = false;
    private PlayerActionController player;
    private float defaultVolume;
    private AudioClip originalAudioClip;
   
    void Start()
    {
        if(isPlayingAudio == true){
            SetPlayAudio(true);
        }
        defaultVolume = audioSource.volume;
        audioSource.volume = 0;

        player = FindObjectOfType<PlayerActionController>();
        if(!player || player == null){
            Debug.LogWarning("DistanceSFXPlayer could not find a Player in scene!");
        }

        originalAudioClip = audioClip;
    }

    
    void Update()
    {
        if(isPlayingAudio && player != null){
            HandleDistanceToPlayer();
        }
        
    }

    private void HandleDistanceToPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if(dist < minDistanceToPlayer){
            audioSource.volume = defaultVolume;
        } else if(dist > maxDistanceToPlayer){
            audioSource.volume = 0f;
        } else {
            audioSource.volume = defaultVolume * (1 - ((dist - minDistanceToPlayer) / (maxDistanceToPlayer - minDistanceToPlayer)));
        }
    }

    public void PlayOneShotClip(AudioClip swapClip)
    {
        audioSource.clip = swapClip;
        audioSource.PlayOneShot(swapClip, audioSource.volume);
    }

    public void SetPlayAudio(bool t){
        isPlayingAudio = t;
        if(t){
            audioSource.clip = audioClip;
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
    }
}
