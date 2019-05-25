using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] GameObject blackFade;
    [SerializeField] GameObject UI;
    
    private void Start() 
    {
        UI.SetActive(false);
        StartCoroutine(StartLevel());
    }
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.tag == "Player" && Input.GetKey(KeyCode.Escape))
        {
            StartCoroutine(EndTheGame());
        }
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(2);
        UI.SetActive(true);
    }
    IEnumerator EndTheGame()
    {
        blackFade.GetComponent<Animator>().SetBool("FadeOut", true);
        UI.SetActive(false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
    }
}
