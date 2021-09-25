using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameMenuHandler : MonoBehaviour
{
    
    [SerializeField] GameObject menuPanel;
    [SerializeField] KeyCode mainMenuInput;

    [SerializeField] TextMeshProUGUI masterVolumeText;
    [SerializeField] TextMeshProUGUI sfxVolumeText;
    [SerializeField] TextMeshProUGUI backgroundVolumeText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get main menu update
        if(Input.GetKeyDown(mainMenuInput)){
            ToggleMenuPanel();
        }
    }

    public void ToggleMenuPanel(){
        menuPanel.SetActive(!menuPanel.activeSelf);
        if(menuPanel.activeSelf){
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1.0f;
        }
    }

    public void SetMasterVolumeValue(int value){
        masterVolumeText.text = value.ToString();
    }

    public void SetSFXVolumeValue(int value){
        sfxVolumeText.text = value.ToString();
    }

    public void SetBGVolumeValue(int value){
        backgroundVolumeText.text = value.ToString();
    }


    
}
