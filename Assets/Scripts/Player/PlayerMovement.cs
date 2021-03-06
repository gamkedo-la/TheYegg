using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    [Header("Player Movement Settings")]
    [SerializeField] bool isLookAtMouse = false;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] bool enableMouseMovement = false;

    //required components
    [Header("Required components")]
    private Rigidbody playerRb;
    private Camera mainCamera;
    [SerializeField] Animator playerAnimator;

    //private
    private float horizontalInput;
    private float verticalInput;
    private Vector3 movementVector;
    private float currentSpeed;
    private Vector3 lookAtPos;
    private Vector3 mousePos;
    //public

    public static PlayerMovement GetPlayer(){
        PlayerMovement playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        return playerMovement;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if(!playerRb || playerRb == null){
            Debug.LogWarning("PlayerMovement component could not find a Rigidbody2D");
        }

        mainCamera = Camera.main;
        if(!mainCamera || mainCamera == null){
            Debug.LogWarning("PlayerMovement component could not find a Main Camera");
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        AnimatePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        movementVector = new Vector3(Mathf.Clamp(horizontalInput, -0.7f, 0.7f), 0f, Mathf.Clamp(verticalInput, -0.7f, 0.7f));
        movementVector.Normalize();
        
        //TODO add checks for sprinting/sneaking
        if(movementVector.magnitude == 0f){
            currentSpeed = 0f;
        } else {
            currentSpeed = walkSpeed;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        playerRb.MovePosition(playerRb.position + movementVector * Time.fixedDeltaTime * currentSpeed);
    }

    private void RotatePlayer(){
        if(isLookAtMouse){
            //controls so that player always faces the mouse position
            //cast a ray from the camera to the scene

            Ray screenRay = mainCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if(Physics.Raycast(screenRay, out hit)){
                if(hit.collider.gameObject.GetComponent<PlayerMovement>()){
                    return;
                } else {
                    lookAtPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                }

            }
            //look at hit position
            transform.LookAt(lookAtPos, Vector3.up);

        } else {
            //controls so that player faces the movement direction
            transform.LookAt(transform.position + movementVector, Vector3.up);
        }
    }

    public float GetPlayerMovementMagnitude(){
        return movementVector.magnitude;
    }

    // TODO: modify once final animations are developed
    // determine how incapacitate shoud operate vs other movement
    // may need to reference PlayerActionController
    // add to update

    private void AnimatePlayer() 
    {
        if(currentSpeed == runSpeed) {
            playerAnimator.SetBool("run", true);
            playerAnimator.SetBool("walk", false);
            playerAnimator.SetBool("idle", false);
            playerAnimator.SetBool("incapacitate", false);
        } else if (currentSpeed == 0f) {
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("walk", false);
            playerAnimator.SetBool("idle", true);
            playerAnimator.SetBool("incapacitate", false);
        } else if (currentSpeed < runSpeed) {
            playerAnimator.SetBool("run", false);
            playerAnimator.SetBool("walk", true);
            playerAnimator.SetBool("idle", false);
            playerAnimator.SetBool("incapacitate", false);
        }
    }

    public void MovePlayerToMousePosition(Vector3 mousePosition)
    {
        if(enableMouseMovement)
        {
            playerRb.MovePosition(playerRb.position + mousePosition * Time.fixedDeltaTime * currentSpeed);
        }
    }
}
