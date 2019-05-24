using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndVillage : MonoBehaviour
{
    [SerializeField] GameObject blackFade;
    [SerializeField] GameObject UI;
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(StartNewLevel());
        }
    }

    IEnumerator StartNewLevel()
    {
        blackFade.GetComponent<Animator>().SetBool("FadeOut", true);
        UI.SetActive(false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("ELR_NewBoss");
    }
}
