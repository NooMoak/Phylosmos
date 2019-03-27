using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryEntry : MonoBehaviour
{

    public GameObject analyseText;
    public GameObject normalUI;
    public GameObject bestiaryUI;
    public GameObject mainPanel;
    public GameObject correspondingPanel;
    public Text panelTitleText1;
    public Text panelTitleText2;
    public RawImage panelIllustration;
    public Text infoText;
    public string desiredTitleText1;
    public string desiredTitleText2;
    public string desiredInfoText;
    public bool imageDiscovered;

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player")
        {
            analyseText.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F))
            {
                FindObjectOfType<CameraController>().GetComponent<CameraController>().normal = false;
                analyseText.SetActive(false);
                normalUI.SetActive(false);
                bestiaryUI.SetActive(true);
                mainPanel.SetActive(false);
                correspondingPanel.SetActive(true);
                panelTitleText1.text = desiredTitleText1;
                panelTitleText2.text = desiredTitleText2;
                infoText.text = desiredInfoText;
                panelIllustration.enabled = imageDiscovered;
                Time.timeScale = 0f;
            }
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
        {
            analyseText.SetActive(false);
        }
    }  
}
