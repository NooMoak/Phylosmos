using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForDeath : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] enemiesToCheck;
    int dead = 0;
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
            StartCoroutine(EndTuto());
        }
    }

    IEnumerator EndTuto()
    {
        yield return null;
        Debug.Log("fini");
    }
    
}
