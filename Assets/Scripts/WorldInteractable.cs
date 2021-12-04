using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldInteractable : MonoBehaviour
{
    //this class shows a TMPro text once the player is inside the trigger area

    [SerializeField] Material outlineMaterial;
    public TextMeshPro promptTextObject;
    public List<GameObject> activatedGameObjects = new List<GameObject>();

    public string promptText;

    private void Start() {
        promptTextObject.text = promptText;
        SetPromptObjectsActive(false);
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            //show TMPro object
            SetPromptObjectsActive(true);
            //add the outline material to list of materials in the mesh renderer
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var m in meshRenderers)
            {
                if(m.CompareTag("InteractionPrompt") == false)
                {
                    List<Material> materials = new List<Material>();
                    materials.Add(m.material);
                    materials.Add(outlineMaterial);
                    m.materials = materials.ToArray();
                }
                
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
           
            SetPromptObjectsActive(false);
            //remove the outlinematerial from list of materials
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var m in meshRenderers)
            {
                if(m.CompareTag("InteractionPrompt") == false)
                {
                    List<Material> materials = new List<Material>();
                    materials.Add(m.materials[0]);
                    m.materials = materials.ToArray();
                }
                
            }
        }

    }

    private void RotateToWorldZ(GameObject g){
        g.transform.SetPositionAndRotation(g.transform.position, Quaternion.LookRotation(Vector3.down, Vector3.up));
    }

    public void SetPromptObjectsActive(bool t)
    {
         foreach(GameObject g in activatedGameObjects)
            {
                g.SetActive(t);
                RotateToWorldZ(g);
            }
    }


}
