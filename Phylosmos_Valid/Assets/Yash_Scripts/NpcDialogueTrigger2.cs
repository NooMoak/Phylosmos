using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialogueTrigger2 : MonoBehaviour
{
    public Dialogue dialogue;
    public static bool InRadius;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InRadius = true;

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InRadius = false;

        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    // Update is called once per frame
    void Update()
    {
        if (InRadius)
        {
            /*if (Input.GetKeyDown(KeyCode.F))
            {
                TriggerDialogue();
            }
            */
            if (NpcPatrol2.waiting2 == true)
            {


                if (Input.GetKeyDown(KeyCode.F))
                {
                    TriggerDialogue();
                }
            }
        }
    }
}
