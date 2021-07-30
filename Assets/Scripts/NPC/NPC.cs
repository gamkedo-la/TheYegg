using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [Header("Required Prefabs")]
    [SerializeField] GameObject deathMark;
    [SerializeField] GameObject downedMark;

    [Header("Required Components")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color defaultNPCColor;
    [SerializeField] GameObject outlineGameObject;

    //private variables
    private Color NPCColor;
    private bool isDown = false;
    private bool hasOutfit = true;

    //public variables
    public bool isNPCDown{
        get {return isDown;}
        set {isDown = isNPCDown;}
    }

    public bool hasNPCOutfit{
        get {return hasOutfit;}
        set {hasOutfit = hasNPCOutfit;}
    }

    private void Start() {
        NPCColor = spriteRenderer.color;
    }
 

    public Color GetNPCColor(){
        return NPCColor;
    }

    public void IncapacitateNPC(){
        Debug.Log("Incapacitated NPC " + name);
        isDown = true;
        //spawn a downed mark on NPC
        Instantiate<GameObject>(downedMark, transform.position, Quaternion.identity);
    }

    public void RemoveNPCOutfit(){
        hasOutfit = false;
        spriteRenderer.color = defaultNPCColor;
    }

    public void SetOutlineActive(bool t){
        outlineGameObject.SetActive(t);
    }

}
