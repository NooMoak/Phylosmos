using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDescriptionManager : MonoBehaviour
{

    private bool squeletteTexteActivated;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && squeletteTexteActivated == false)
        {
            DescriptionManager.MyInstance.SqueletteTexte();
            squeletteTexteActivated = true;
        }
    }
}
