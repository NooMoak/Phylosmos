using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeOfEntry
{
    Spike, Liana, Healer, Rock, Villager, FireBoss
}
public class BestiaryEntry : MonoBehaviour
{
    public TypeOfEntry entryFor;
    public GameObject analyseText;
    public GameObject normalUI;
    public GameObject bestiaryUI;
    public GameObject mainPanel;
    public GameObject correspondingPanel; 
    [SerializeField] BestiaryData bestiaryData;
    bool pressed = false;
    GameObject scan;

    void Start() 
    {
        scan = transform.Find("Scan").gameObject;
    }

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player")
        {
            analyseText.SetActive(true);
            analyseText.GetComponent<Text>().text = "Press 'F' to analyse";
            if(Input.GetKeyDown(KeyCode.F) && pressed == false)
            {
                pressed = true;
                scan.GetComponent<ParticleSystem>().Play();
                if(entryFor == TypeOfEntry.Spike)
                {
                    bestiaryData.SpikeEntry();
                }
                else if(entryFor == TypeOfEntry.Liana)
                {
                    bestiaryData.LianaEntry();
                }
                else if(entryFor == TypeOfEntry.Healer)
                {
                    bestiaryData.HealerEntry();
                }
                else if(entryFor == TypeOfEntry.Rock)
                {
                    bestiaryData.RockEntry();
                }
                else if(entryFor == TypeOfEntry.Villager)
                {
                    bestiaryData.VillagerEntry();
                }
                else if(entryFor == TypeOfEntry.FireBoss)
                {
                    bestiaryData.BossEntry();
                }
                else
                {
                    Debug.Log("Entry For is not assigned, please assign it in the script component");
                }
                StartCoroutine(Entry());
            }
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
        {
            analyseText.SetActive(false);
        }
    }  

    IEnumerator Entry()
    {
        yield return new WaitForSeconds(3);
        FindObjectOfType<CameraController>().GetComponent<CameraController>().normal = false;
        analyseText.SetActive(false);
        normalUI.SetActive(false);
        bestiaryUI.SetActive(true);
        mainPanel.SetActive(false);
        correspondingPanel.SetActive(true);
        //panelTitleText1.text = desiredTitleText1;
        //panelTitleText2.text = desiredTitleText2;
        //infoText.text = desiredInfoText;
        //panelIllustration.enabled = imageDiscovered;
        Time.timeScale = 0f;
        Destroy(this.gameObject);
    }
}
