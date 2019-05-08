using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCam : MonoBehaviour
{

    [SerializeField] GameObject normalCam;
    [SerializeField] GameObject introCam;
    [SerializeField] GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.gameObject.GetComponent<CameraController>().exeption = true;
        UI.SetActive(false);
        StartCoroutine(CamMove());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CamMove()
    {
        yield return new WaitForSeconds(2);
        introCam.SetActive(false);
        Camera.main.gameObject.GetComponent<CameraController>().exeption = false;
        Camera.main.gameObject.GetComponent<CameraController>().NormalCam();
        UI.SetActive(true);
    }
}
