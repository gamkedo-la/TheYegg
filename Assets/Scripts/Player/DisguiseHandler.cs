using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisguiseHandler : MonoBehaviour
{

    [Header("Handler settings")]
    [SerializeField] Color defaultOutfitColour = Color.white;


    //private
    private Color currentOutfitColour;

    //public

    //required components
    [Header("Required components")]
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {
        currentOutfitColour = defaultOutfitColour;
        spriteRenderer.color = currentOutfitColour;

    }

    public void EquipDisguise(NPC n){
        //on input, get closest NPC in handler
        //get the disguise of that NPC
        if(!n || n == null){
            Debug.Log("No downed NPC found!");
        } else if(n.hasNPCOutfit == true) {
            Debug.Log("equipping disguise!");
            Color npcColor = n.GetNPCColor();
            spriteRenderer.color = npcColor;
            n.RemoveNPCOutfit();
        } else {
            Debug.Log("Downed NPC does not have an outfit anymore!");
        }

    }
}
