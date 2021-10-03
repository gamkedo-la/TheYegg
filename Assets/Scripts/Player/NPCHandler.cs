using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    [Header("Required components")]
    [SerializeField] BoxCollider boxCollider;

    //private

    private Transform parentTransform;
    private NPC highlightedNPC = null;

    //public

    public List<NPC> npcList = new List<NPC>();
    void Start()
    {
        parentTransform = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //keep closest NPC highlighted
        if(npcList.Count > 0){
            
            NPC closestNPC = GetClosestNPC();
            if(highlightedNPC == null){
                highlightedNPC = closestNPC;
                highlightedNPC.SetOutlineActive(true);
            } else if(closestNPC != highlightedNPC){
                highlightedNPC.SetOutlineActive(false);
                highlightedNPC = closestNPC;
                highlightedNPC.SetOutlineActive(true);
            } else {
                closestNPC = null;
            }
        } else {
            highlightedNPC = null;
        }

    }

    private void OnTriggerEnter(Collider other) {
        //if a new NPC enters the trigger, add it to list
        if(other.TryGetComponent<NPC>(out NPC n)){
            if(!npcList.Contains(n)){
                npcList.Add(n);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        //remove from list when exits trigger
        if(other.TryGetComponent<NPC>(out NPC n)){
            n.SetOutlineActive(false);
            npcList.Remove(n);
        }
    }

    public NPC GetClosestNPC(){
        if(npcList.Count <= 0){
            return null;
        } else {
            NPC closestNPC = null;
            float compDist = Mathf.Infinity;
            foreach(NPC n in npcList){
                float nDist = Vector2.Distance(parentTransform.position, n.transform.position);
                if(nDist < compDist){
                    compDist = nDist;
                    closestNPC = n;
                }
            }
            return closestNPC;
        }
    }

    
    public NPC GetClosestDownedNPC(){
        if(npcList.Count <= 0){
            return null;
        } else {
            NPC closestNPC = null;
            float compDist = Mathf.Infinity;
            foreach(NPC n in npcList){
                if(n.isNPCDown == true){
                    float nDist = Vector2.Distance(parentTransform.position, n.transform.position);
                    if(nDist < compDist){
                        compDist = nDist;
                        closestNPC = n;
                    }
                }
                
            }
            return closestNPC;
        }
    }

    public void ResetNPCList(){
        npcList = new List<NPC>();
    }
}
