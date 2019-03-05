using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public GameObject normalUI;
    public GameObject bestiaryUI;
    bool normal = true;

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
            normalUI.SetActive(true);
            bestiaryUI.SetActive(false);
            Time.timeScale = 1f;
            normal = true;
        }
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}