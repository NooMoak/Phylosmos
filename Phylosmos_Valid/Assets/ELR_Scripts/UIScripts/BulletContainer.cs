using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletContainer : MonoBehaviour
{
    [SerializeField] Text magazineText;
    [SerializeField] Image bullet1;
    [SerializeField] Image bullet2;
    [SerializeField] Image bullet3;
    [SerializeField] Image bullet4;
    [SerializeField] Image bullet5;
    [SerializeField] Image bullet6;
    [SerializeField] Image bullet7;
    [SerializeField] Image bullet8;
    [SerializeField] Image bullet9;
    [SerializeField] Image bullet10;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(magazineText.text == "10")
        {
            bullet1.enabled = true;
            bullet2.enabled = true;
            bullet3.enabled = true;
            bullet4.enabled = true;
            bullet5.enabled = true;
            bullet6.enabled = true;
            bullet7.enabled = true;
            bullet8.enabled = true;
            bullet9.enabled = true;
            bullet10.enabled = true;
        }

        if(magazineText.text == "09")
        {
            bullet10.enabled = false;
        }

        if(magazineText.text == "08")
        {
            bullet9.enabled = false;
        }

        if(magazineText.text == "07")
        {
            bullet8.enabled = false;
        }

        if(magazineText.text == "06")
        {
            bullet7.enabled = false;
        }

        if(magazineText.text == "05")
        {
            bullet6.enabled = false;
        }

        if(magazineText.text == "04")
        {
            bullet5.enabled = false;
        }

        if(magazineText.text == "03")
        {
            bullet4.enabled = false;
        }

        if(magazineText.text == "02")
        {
            bullet3.enabled = false;
        }

        if(magazineText.text == "01")
        {
            bullet2.enabled = false;
        }

        if(magazineText.text == "00")
        {
            bullet1.enabled = false;
        }
    }
}
