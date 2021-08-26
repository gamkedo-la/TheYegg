using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCameraRotator : MonoBehaviour
{
    
    [Header("Rotation settings")]
    [SerializeField] float rotationToRight;
    [SerializeField] float rotationToLeft;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float directionChangeController = 1f;

    

    //private
    public float adjustedMinAngleLimit;
    public bool canChangeDirection = false;
    private float startAngle;

    public float localRotationY;
    public bool isMovingRight = true;
    public Vector3 fwVector = Vector3.zero;
    public float diffAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        startAngle = transform.localEulerAngles.y;
        fwVector = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        
    }

    private void RotateCamera(){
        localRotationY = transform.localRotation.eulerAngles.y;

        //start by moving right
        //Debug.Log("Reached angle is "+ Vector3.Angle(fwVector, transform.forward));
        diffAngle = Vector3.Angle(fwVector, transform.forward);
        transform.RotateAround(transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        if(isMovingRight){
            if(Vector3.Angle(fwVector, transform.forward) >= rotationToRight + rotationToLeft){
                //change direction
                rotationSpeed = rotationSpeed * -1;
                isMovingRight = false;
                fwVector = transform.forward;
            }
        } else {
            //moving left until local y rotation is non-negative and bigger than min angle limit
            
            if(Vector3.Angle(fwVector, transform.forward) >= rotationToRight + rotationToLeft){
                //change direction
                rotationSpeed = rotationSpeed * -1;
                isMovingRight = true;
                fwVector = transform.forward;
            }
        }

    }
}
