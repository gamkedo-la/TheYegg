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
    [SerializeField] Animator playerAnim;
    [SerializeField] PlayerAnimationController playerAnimationController;
    private PlayerActionController playerActionController;

    void Start()
    {
        playerActionController = GetComponent<PlayerActionController>();
        if(!playerActionController || playerActionController == null){
            Debug.LogWarning("No PlayerActionController component found in DisguiseHandler!");
        }
    }

    public void EquipDisguise(NPC n){
        //on input, get closest NPC in handler
        //get the disguise of that NPC
        if(!n || n == null){
            Debug.Log("No downed NPC found!");
        } else if(n.hasNPCOutfit == true) {
            Debug.Log(n.nPCDisguiseAnimationController.name);
            Debug.Log("equipping disguise!");
            FindObjectOfType<ScoreKeeper>().IncreaseDisguisesUsed();
            AnimatorOverrideController npcAnim = n.GetDisguiseAnimator();
            if(npcAnim != playerAnim.runtimeAnimatorController){
                //equipped different disguise
                Debug.Log("Equipped different disguise!");
                playerActionController.SetIsDisguiseCompromised(false);
                FindObjectOfType<HUDHandler>().SetDisguiseStatus(false);
            }
            //currentOutfitColour = npcColor;
            //spriteRenderer.color = npcColor;
            playerAnimationController.SetAnimator(n.GetDisguiseAnimator());
            n.RemoveNPCOutfit();
        } else {
            Debug.Log("Downed NPC does not have an outfit anymore!");
        }

    }

    public void ResetDisguiseHandler(){
        //currentOutfitColour = defaultOutfitColour;
        //spriteRenderer.color = currentOutfitColour;
        playerAnimationController.ResetAnimator();
        playerActionController.SetIsDisguiseCompromised(false);
        HUDHandler hUDHandler = FindObjectOfType<HUDHandler>();
        if(hUDHandler){
            FindObjectOfType<HUDHandler>().SetDisguiseStatus(false);
        }
    }

    public void CompromiseDisguise(AnimatorOverrideController disguiseAnimator){
        if(disguiseAnimator == playerAnimationController.GetCurrentAnimator()){
            //set disguise compromised in player action controller
            playerActionController.SetIsDisguiseCompromised(true);
            FindObjectOfType<HUDHandler>().SetDisguiseStatus(true);
        }
    }
}
