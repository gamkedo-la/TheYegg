using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public bool Enabled = false;
    public GameObject theMap;
    
    void Start()
    {
        
    }

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.M)) {
             Enabled = !Enabled; // toggle
             Debug.Log("MiniMap is "+(Enabled?"ON":"OFF"));
             if (theMap) theMap.SetActive(Enabled);
         }
    }
}
