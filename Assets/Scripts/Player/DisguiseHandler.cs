using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisguiseHandler : MonoBehaviour
{

    [Header("Handler settings")]
    [SerializeField] Color defaultOutfitColour = Color.white;


    //private
    public Color currentOutfitColour;

    //public

    //required components
    [Header("Required components")]
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {

    }

    public void EquipDisguise(NPC n){
        //on input, get closest NPC in handler
        //get the disguise of that NPC
        if(!n || n == null){
            Debug.Log("No downed NPC found!");
        } else if(n.hasNPCOutfit == true) {
            Debug.Log("equipping disguise!");
            Color npcColor = n.GetNPCColor();
            if(npcColor != spriteRenderer.color){
                //equipped different disguise
                Debug.Log("Equipped different disguise!");
                GetComponent<PlayerActionController>().SetIsDisguiseCompromised(false);
                FindObjectOfType<HUDHandler>().SetDisguiseStatus(false);
            }
            currentOutfitColour = npcColor;
            spriteRenderer.color = npcColor;
            n.RemoveNPCOutfit();
        } else {
            Debug.Log("Downed NPC does not have an outfit anymore!");
        }

    }

    public void ResetDisguiseHandler(){
        currentOutfitColour = defaultOutfitColour;
        spriteRenderer.color = currentOutfitColour;
    }

    public void CompromiseDisguise(Color disguiseColor){
        PlayerActionController playerActionController = GetComponent<PlayerActionController>();
        if(disguiseColor == currentOutfitColour){
            //set disguise compromised in player action controller
            playerActionController.SetIsDisguiseCompromised(true);
            FindObjectOfType<HUDHandler>().SetDisguiseStatus(true);
        }
    }
}
