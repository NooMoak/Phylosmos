using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckForDeath : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] enemiesToCheck;
    [SerializeField] GameObject analyseText;
    [SerializeField] GameObject blackFade;
    [SerializeField] GameObject UI;
    int dead = 0;
    bool dialogueTriggered;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in enemiesToCheck)
        {
            if(enemy.tag == "Liana" && enemy.GetComponent<LianaBehavior>().currentState == LianaState.Dead)
                dead += 1;
            if(enemy.tag == "Spike" && enemy.GetComponent<SpikeBehavior>().currentState == SpikeState.Dead)
                dead += 1;
            if(enemy.tag == "Healer" && enemy.GetComponent<HealerBehavior>().currentState == HealerState.Dead)
                dead += 1;
            if(enemy.tag == "Rock" && enemy.GetComponent<RockBehavior>().currentState == RockState.Dead)
                dead += 1;
        }

        if(dead == enemiesToCheck.Length)
        {
            GetComponent<DialogueTrigger>().TriggerDialogue();
            dialogueTriggered = true;
        }

        if(dialogueTriggered == true && analyseText.activeSelf == true)
        {
            StartCoroutine(StartNewLevel());
        }
    }

    IEnumerator StartNewLevel()
    {
        blackFade.GetComponent<Animator>().SetBool("FadeOut", true);
        UI.SetActive(false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("JOB_Lvl forêt");
    }
    
}
