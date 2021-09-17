using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private string objectID;



    private void Awake() {
        objectID = name + transform.position.ToString();
    }

    private void Start() {
        for (int i = 0; i < GameObject.FindObjectsOfType<DontDestroy>().Length; i++){
            if(GameObject.FindObjectsOfType<DontDestroy>()[i] != this){
                if(GameObject.FindObjectsOfType<DontDestroy>()[i].name == gameObject.name){
                Destroy(gameObject);
                }
            }   
        } 
        DontDestroyOnLoad(gameObject);
    }
}
