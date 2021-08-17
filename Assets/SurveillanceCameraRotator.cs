using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCameraRotator : MonoBehaviour
{
    
    [Header("Rotation settings")]
    [SerializeField] float maxAngleLimit = 45f;
    [SerializeField] float minAngleLimit = 45f;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float directionChangeController = 1f;

    

    //private
    private float adjustedMinAngleLimit;
    public bool canChangeDirection = false;
    private float startAngle;

    public float localRotationY;

    
    // Start is called before the first frame update
    void Start()
    {
        startAngle = transform.localEulerAngles.y;
        adjustedMinAngleLimit = 360 - Mathf.Abs(minAngleLimit);
        minAngleLimit = startAngle - minAngleLimit;
        maxAngleLimit = startAngle + maxAngleLimit;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        
    }

    private void RotateCamera(){
        localRotationY = transform.localRotation.eulerAngles.y;
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        if(localRotationY > maxAngleLimit && canChangeDirection){
            //turn rotation direction by adjusting rotationspeed
            rotationSpeed = rotationSpeed * -1;
            canChangeDirection = false;
        }

        if(localRotationY < minAngleLimit && canChangeDirection){
            rotationSpeed = rotationSpeed * -1;
            canChangeDirection = false;
        }

        if(localRotationY < maxAngleLimit && localRotationY > minAngleLimit){
            canChangeDirection = true;
        }

    }

/*
        if(canChangeDirection)
        {
            if(transform.localRotation.eulerAngles.y > maxAngleLimit && transform.localRotation.eulerAngles.y < adjustedMinAngleLimit)
            {
                canChangeDirection = false;
                rotationSpeed = -rotationSpeed;
            }
        }

        if(Mathf.DeltaAngle(transform.localRotation.eulerAngles.y, (maxAngleLimit + minAngleLimit) / 2) < directionChangeController && Mathf.DeltaAngle(transform.localRotation.eulerAngles.y, (maxAngleLimit + minAngleLimit) / 2) > -directionChangeController){
            canChangeDirection = true;
        }
    }
    */
}
