using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryData : MonoBehaviour
{
    public bool spike1 = false;
    public bool spike2 = false;
    public bool spike3 = false;
    public bool liana1 = false;
    public bool liana2 = false;
    public bool liana3 = false;
    public bool healer1 = false;
    public bool healer2 = false;
    public bool healer3 = false;
    public bool rock1 = false;
    public bool rock2 = false;
    public bool rock3 = false;
    public bool villager1 = false;
    public bool villager2 = false;
    public bool villager3 = false;
    public bool villager4 = false;
    public bool villager5 = false;
    public bool villager6 = false;
    public bool villager7 = false;
    public bool boss1 = false;
    public bool boss2 = false;
    public bool boss3 = false;
    DataSaver dataSaver;
    [SerializeField] GameObject analysisUI;
    [SerializeField] Image analysisJauge;
    [SerializeField] Text analysisText;
    [SerializeField] Text analysisStep;

    float targetedAmount = 0f;
    // Start is called before the first frame update
    void Start()
    {
        dataSaver = FindObjectOfType<DataSaver>();
        spike1 = dataSaver.spike1;
        spike2 = dataSaver.spike2;
        spike3 = dataSaver.spike3;
        liana1 = dataSaver.liana1;
        liana2 = dataSaver.liana2;
        liana3 = dataSaver.liana3;
        healer1 = dataSaver.healer1;
        healer2 = dataSaver.healer2;
        healer3 = dataSaver.healer3;
        rock1 = dataSaver.rock1;
        rock2 = dataSaver.rock2;
        rock3 = dataSaver.rock3;
        villager1 = dataSaver.villager1;
        villager2 = dataSaver.villager2;
        villager3 = dataSaver.villager3;
        villager4 = dataSaver.villager4;
        villager5 = dataSaver.villager5;
        villager6 = dataSaver.villager6;
        villager7 = dataSaver.villager7;
        boss1 = dataSaver.boss1;
        boss2 = dataSaver.boss2;
        boss3 = dataSaver.boss3;
    }

    // Update is called once per frame
    void Update()
    {
        if(analysisJauge.fillAmount != targetedAmount)
        {
            analysisJauge.fillAmount = Mathf.Lerp(analysisJauge.fillAmount, targetedAmount, 0.1f);
        }
    }

    public void SpikeEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(spike1 == false)
        {
            spike1 = true;
            dataSaver.spike1 = true;
            analysisText.text = "Thorny_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(spike2 == false)
        {
            spike2 = true;
            dataSaver.spike2 = true;
            analysisText.text = "Thorny_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(spike3 == false)
        {
            spike3 = true;
            dataSaver.spike3 = true;
            analysisText.text = "Thorny_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
    }

    public void LianaEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(liana1 == false)
        {
            liana1 = true;
            dataSaver.liana1 = true;
            analysisText.text = "Liana_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(liana2 == false)
        {
            liana2 = true;
            dataSaver.liana2 = true;
            analysisText.text = "Liana_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(liana3 == false)
        {
            liana3 = true;
            dataSaver.liana3 = true;
            analysisText.text = "Liana_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
    }
    
    public void HealerEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(healer1 == false)
        {
            healer1 = true;
            dataSaver.healer1 = true;
            analysisText.text = "Healer_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(healer2 == false)
        {
            healer2 = true;
            dataSaver.healer2 = true;
            analysisText.text = "Healer_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(healer3 == false)
        {
            healer3 = true;
            dataSaver.healer3 = true;
            analysisText.text = "Healer_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
    }
    
    public void RockEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(rock1 == false)
        {
            rock1 = true;
            dataSaver.rock1 = true;
            analysisText.text = "Guardian_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(rock2 == false)
        {
            rock2 = true;
            dataSaver.rock2 = true;
            analysisText.text = "Guardian_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(rock3 == false)
        {
            rock3 = true;
            dataSaver.rock3 = true;
            analysisText.text = "Guardian_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
    }
    
    public void VillagerEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(villager1 == false)
        {
            villager1 = true;
            dataSaver.villager1 = true;
            analysisText.text = "Villager_analysis";
            analysisStep.text = "1 / 7 step";
            targetedAmount = 0.14f;
        }
        else if(villager2 == false)
        {
            villager2 = true;
            dataSaver.villager2 = true;
            analysisText.text = "Villager_analysis";
            analysisStep.text = "2 / 7 step";
            targetedAmount = 0.28f;
        }
        else if(villager3 == false)
        {
            villager3 = true;
            dataSaver.villager3 = true;
            analysisText.text = "Villager_analysis";
            analysisStep.text = "3 / 7 step";
            targetedAmount = 0.42f;
        }
        else if(villager4 == false)
        {
            villager4 = true;
            dataSaver.villager4 = true;
            analysisText.text = "Villager_analysis";
            analysisStep.text = "4 / 7 step";
            targetedAmount = 0.57f;
        }
        else if(villager5 == false)
        {
            villager5 = true;
            dataSaver.villager5 = true;
            analysisText.text = "Villager_analysis";
            analysisStep.text = "5 / 7 step";
            targetedAmount = 0.71f;
        }
        else if(villager6 == false)
        {
            villager6 = true;
            dataSaver.villager6 = true;
            analysisText.text = "Villager_analysis";
            analysisStep.text = "6 / 7 step";
            targetedAmount = 0.85f;
        }
        else if(villager7 == false)
        {
            villager7 = true;
            dataSaver.villager7 = true;
            analysisText.text = "Villager_analysis";
            analysisStep.text = "7 / 7 step";
            targetedAmount = 1f;
        }
    }
    
    public void BossEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(boss1 == false)
        {
            boss1 = true;
            dataSaver.boss1 = true;
            analysisText.text = "???_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(boss2 == false)
        {
            boss2 = true;
            dataSaver.boss2 = true;
            analysisText.text = "???_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(boss3 == false)
        {
            boss3 = true;
            dataSaver.boss3 = true;
            analysisText.text = "???_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(3);
        analysisUI.SetActive(false);
    }
}
