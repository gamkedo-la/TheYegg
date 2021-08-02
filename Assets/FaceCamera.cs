using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    private Camera mainCamera;

    private void Start() {
        //find main camera
        mainCamera = Camera.main;
        if(!mainCamera || mainCamera == null){
            Debug.LogWarning("No main camera found!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        Vector3 oppositeCamera = transform.parent.position - mainCamera.transform.position;
        Quaternion cameraRot = Quaternion.LookRotation(oppositeCamera);
        Vector3 euler = cameraRot.eulerAngles;
        euler.y = 0f;
        cameraRot.eulerAngles = euler;
        transform.rotation = cameraRot;
    }
}
