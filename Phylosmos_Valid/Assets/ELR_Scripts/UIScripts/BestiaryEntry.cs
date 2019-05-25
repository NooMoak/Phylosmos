using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TypeOfEntry
{
    Spike, Liana, Healer, Rock, VillagerField, VillagerTower, VillagerLanguage, VillagerDrawings, VillagerChief, FireBoss, End
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
    [SerializeField] GameObject scan;
    [SerializeField] AudioClip audioAnalyze;

    void Start() 
    {
        //scan = transform.Find("Scan").gameObject;
    }

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player")
        {
            analyseText.SetActive(true);
            analyseText.GetComponent<Text>().text = "Press \"F\" to analyse";
            if(Input.GetKeyDown(KeyCode.F) && pressed == false)
            {
                pressed = true;
                scan.GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().clip = audioAnalyze;
                GetComponent<AudioSource>().Play();
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
                else if(entryFor == TypeOfEntry.VillagerField)
                {
                    bestiaryData.villagerField = true;
                    bestiaryData.VillagerEntry();
                }
                else if(entryFor == TypeOfEntry.VillagerTower)
                {
                    bestiaryData.villagerTower = true;
                    bestiaryData.VillagerEntry();
                }
                else if(entryFor == TypeOfEntry.VillagerLanguage)
                {
                    bestiaryData.villagerLanguage = true;
                    bestiaryData.VillagerEntry();
                }
                else if(entryFor == TypeOfEntry.VillagerDrawings)
                {
                    bestiaryData.villagerDrawings = true;
                    bestiaryData.VillagerEntry();
                }
                else if(entryFor == TypeOfEntry.VillagerChief)
                {
                    bestiaryData.villagerChief = true;
                    bestiaryData.VillagerEntry();
                }
                else if(entryFor == TypeOfEntry.FireBoss)
                {
                    bestiaryData.BossEntry();
                }
                else if(entryFor == TypeOfEntry.End)
                {
                    bestiaryData.EndEntry();
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
        yield return new WaitForSeconds(2);
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
        //Time.timeScale = 0f;
        Destroy(this.gameObject);
    }
}
