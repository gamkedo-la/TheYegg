using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelTransition : MonoBehaviour
{

    [SerializeField] int toLevelIndex;
    [SerializeField] string toLevelName;
    [SerializeField] GameObject elevatorUI;
    [SerializeField] TextMeshProUGUI elevatorText;
    [SerializeField] float xOffset;
    [SerializeField] float yOffset;
    [SerializeField] float smoothTime = .3f;
    [SerializeField] float maxSmoothSpeed = 2f;


    private RectTransform textRectTransform;
    private Camera mainCamera;
    private Vector2 currentVelocity;

    private void Awake() {
        elevatorUI.SetActive(false);
        elevatorText.text = "Go to " + toLevelName;

        textRectTransform = elevatorUI.GetComponent<RectTransform>();
        if(!textRectTransform || textRectTransform == null){
            Debug.LogWarning("Level Transition for " + gameObject.name + " could not find a RectTransform!");
        }

        mainCamera = Camera.main;
        if(!mainCamera || mainCamera == null){
            Debug.LogWarning("Level Transition for " + gameObject.name + " could not find a Main Camera!");
        }

        //set elevator text UI pos to starting position
        Vector3 elevatorPositionOnScreen = mainCamera.WorldToScreenPoint(this.transform.position);
        Vector2 elevatorPos = new Vector2(elevatorPositionOnScreen.x + xOffset, elevatorPositionOnScreen.y + yOffset);
        textRectTransform.anchoredPosition = elevatorPos;
    }

    private void Update() {
        
        //Debug.Log("Elevator pos in screen coordinates is " + mainCamera.WorldToScreenPoint(this.transform.position));
        SetElevatorUIPosition();

    }

    private void SetElevatorUIPosition()
    {
        Vector3 elevatorPositionOnScreen = mainCamera.WorldToScreenPoint(this.transform.position);
        Vector2 elevatorPos = new Vector2(elevatorPositionOnScreen.x + xOffset, elevatorPositionOnScreen.y + yOffset);
        textRectTransform.anchoredPosition = Vector2.SmoothDamp(textRectTransform.anchoredPosition, elevatorPos, ref currentVelocity, smoothTime, maxSmoothSpeed);
    }

    private void ToggleElevatorUI(bool t){
        elevatorUI.SetActive(t);
    }

    public void TransitionToLevel(){
         SceneManager.LoadScene(toLevelIndex);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            ToggleElevatorUI(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            ToggleElevatorUI(false);
        }
    }


}
