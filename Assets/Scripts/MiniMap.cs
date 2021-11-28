using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public bool Enabled = false;
    public GameObject theMap;

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.V)) {
             Enabled = !Enabled; // toggle
             Debug.Log("MiniMap is "+(Enabled?"ON":"OFF"));
             if (theMap) theMap.SetActive(Enabled);
         }
    }
}
