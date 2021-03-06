using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC settings")]
    [SerializeField] Color defaultNPCColor;

    [Header("Required Prefabs")]
    [SerializeField] GameObject downedMark;

    [Header("References to other Gameobjects")]
    [SerializeField] GameObject fov;    

    [Header("Required Components")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] KeySpawner keySpawner;
    [SerializeField] GuardFSM guardFSM;

    [Header("NPC Outfit settings")]
    public AnimatorOverrideController nPCDisguiseAnimationController;
    


    //private variables
    private Color NPCColor;
    public bool isDown = false;
    private bool hasOutfit = true;
    private GameObject downedMarkObject;
    public float npcSpeed;

    //public variables
    public bool isNPCDown{
        get {return isDown;}
        set {isDown = isNPCDown;}
    }

    public bool hasNPCOutfit{
        get {return hasOutfit;}
        set {hasOutfit = hasNPCOutfit;}
    }

    public Color defaultNPCOutfit{
        get {return defaultNPCColor;}
    }

    private void Start() {
        NPCColor = spriteRenderer.color;
    }

    public AnimatorOverrideController GetDisguiseAnimator()
    {
        return nPCDisguiseAnimationController;
    }

    public Color GetNPCColor(){
        return NPCColor;
    }

    public void IncapacitateNPC(){
        //Debug.Log("Incapacitated NPC " + name);
        isDown = true;
        //spawn a downed mark on NPC
        FindObjectOfType<ScoreKeeper>().IncreaseGuardsSubdued();
        downedMarkObject = Instantiate<GameObject>(downedMark, this.transform);
        //when NPC is incapacitated, disable FOV
        fov.SetActive(false);
        keySpawner.SpawnKey();
        guardFSM.PushState(guardFSM.incapacitatedState);
        guardFSM.activeState.EndGuardState();
    }

    public void RemoveNPCOutfit(){
        hasOutfit = false;
        spriteRenderer.color = defaultNPCColor;
    }

    public void ReactivateNPC(){
        isDown = false;
        Destroy(downedMarkObject);
        fov.SetActive(true);
        fov.gameObject.GetComponent<FieldOfView>().ReactivateFOV();
        FindObjectOfType<PlayerActionController>().SetIsDisguiseCompromised(true);
        FindObjectOfType<HUDHandler>().SetDisguiseStatus(true);
        //reset some outfit?
    }

}
