using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSequenceObject : MonoBehaviour
{
    [Header("Interaction settings")]
    [SerializeField] float timeToInteract;
    [SerializeField] Slider slider;
    [Tooltip("What is the position of this object in the correct sequence of clearing the level")]
    [SerializeField] int positionInSequence;
    [Header("Object Highlightiong settings")]
    [SerializeField] bool isHighlighted = false;
    [SerializeField] GameObject highlightedObject;
    private Material defaultMaterial;
    [SerializeField] Material highlightMaterial;

    private MeshRenderer meshRenderer;

    private void Start() {
        if(isHighlighted && highlightedObject != null)
        {
            //save default material
            meshRenderer = highlightedObject.GetComponent<MeshRenderer>();
            defaultMaterial = meshRenderer.material;
            //set effect material as material
            meshRenderer.material = highlightMaterial;

        }
    }


    public void InteractWithObject(){
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if(levelManager){
            //if has cleared previous condition, set this condition to be cleared
            if(levelManager.GetLevelClearConditionCompleted() == positionInSequence - 1){
                WorldInteractable worldInteractable;
                BoxCollider boxCollider;
                AudioSource audioSource;

                if(TryGetComponent<WorldInteractable>(out worldInteractable))
                {
                    worldInteractable.SetPromptObjectsActive(false);
                }
                if(TryGetComponent<BoxCollider>(out boxCollider))
                {
                    boxCollider.enabled = false;
                }
                if(TryGetComponent<AudioSource>(out audioSource))
                {
                    audioSource.Play();
                }
                if(isHighlighted)
                {
                    meshRenderer.material = defaultMaterial;
                }
                levelManager.LevelClearConditionCompleted(positionInSequence);
            }
        }
    }

    public float GetTimeToInteract()
    {
        return timeToInteract;
    }

    public void SetTimerValue(float v){
        slider.value = v;
    }

}
