using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public GameObject normalUI;
    public GameObject bestiaryUI;
    public GameObject mainPanel;
    public GameObject miraPanel;
    public GameObject nativesPanel;
    public GameObject lianaPanel;
    public GameObject spikePanel;
    public GameObject healerPanel;
    public GameObject rockPanel;
    public GameObject bossPanel;
    [SerializeField] GameObject introText;
    public bool normal = true;
    [SerializeField] AudioClip audioOpen;

    public GameObject normalCam;
    public GameObject fightCam;
    public GameObject spellCam;
    public GameObject hurtCam;
    public bool exeption;
    bool isOpen = false;

    void Start()
    {

    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Tab) && normal == true)
        {
            normalUI.SetActive(false);
            bestiaryUI.SetActive(true);
            normal = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && normal == false)
        {
            QuitBestiary();
        }
        if(normal == false)
        {
            if(isOpen == false)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetBool("Moving", false);
                GetComponent<AudioSource>().clip = audioOpen;
                GetComponent<AudioSource>().Play();
                StartCoroutine(BestiaryOpen());
            }
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentState = PlayerState.Stagger;
        }
    }

    void QuitBestiary()
    {
        mainPanel.SetActive(true);
        miraPanel.SetActive(false);
        nativesPanel.SetActive(false);
        lianaPanel.SetActive(false);
        spikePanel.SetActive(false);
        healerPanel.SetActive(false);
        rockPanel.SetActive(false);
        bossPanel.SetActive(false);
        normalUI.SetActive(true);
        bestiaryUI.SetActive(false);
        Time.timeScale = 1f;
        normal = true;
        isOpen = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentState = PlayerState.Idle;
        introText.SetActive(false);  
        bestiaryUI.GetComponent<Animator>().SetTrigger("Close");
    }

    public void NormalCam() 
    {
        if(normalCam.activeSelf == false && exeption == false)
        {
            fightCam.SetActive(false);
            spellCam.SetActive(false);
            hurtCam.SetActive(false);
            normalCam.SetActive(true);
        }
    }

    public void FightCam() 
    {
        if(fightCam.activeSelf == false && spellCam.activeSelf == false && hurtCam.activeSelf == false)
        {
            spellCam.SetActive(false);
            hurtCam.SetActive(false);
            normalCam.SetActive(false);
            fightCam.SetActive(true);
        }
    }

    public void SpellCam() 
    {
        if(spellCam.activeSelf == false)
        {
            hurtCam.SetActive(false);
            normalCam.SetActive(false);
            fightCam.SetActive(false);
            spellCam.SetActive(true);
        }
    }

    public void HurtCam() 
    {
        if(hurtCam.activeSelf == false)
        {
            fightCam.SetActive(false);
            spellCam.SetActive(false);
            normalCam.SetActive(false);
            hurtCam.SetActive(true);
            StartCoroutine(ReturnToNormal());
        }
    }

    IEnumerator ReturnToNormal()
    {
        yield return new WaitForSeconds(0.5f);
        NormalCam();
    }

    IEnumerator BestiaryOpen()
    {
        isOpen = true;
        bestiaryUI.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }
}