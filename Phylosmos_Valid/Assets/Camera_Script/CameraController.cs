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
    public bool normal = true;

    private Vector3 offset;

    void Start()
    {
        //Cursor.visible = false;
        offset = transform.position - player.transform.position;
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Tab) && normal == true)
        {
            normalUI.SetActive(false);
            bestiaryUI.SetActive(true);
            Time.timeScale = 0f;
            normal = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && normal == false)
        {
            QuitBestiary();
        }
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
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
    }
}