using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public bool Enabled = false;
    public GameObject theMap;

    private void Start() {
        SetMiniMapActive(true);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) {
            SetMiniMapActive(!Enabled);
            
        }
    }

    private void SetMiniMapActive(bool t)
    {
        Enabled = t;
        Debug.Log("MiniMap is "+(Enabled?"ON":"OFF"));
        if (theMap) theMap.SetActive(Enabled);
    }
}
