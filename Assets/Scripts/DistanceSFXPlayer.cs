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
    private PlayerActionController player;
    private float defaultVolume;
   
    void Start()
    {
        defaultVolume = audioSource.volume;
        audioSource.volume = 0;
        player = FindObjectOfType<PlayerActionController>();
        if(!player || player == null){
            Debug.LogWarning("DistanceSFXPlayer could not find a Player in scene!");
        }
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    
    void Update()
    {
        HandleDistanceToPlayer();
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
}
