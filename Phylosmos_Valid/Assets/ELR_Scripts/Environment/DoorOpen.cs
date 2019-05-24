using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] GameObject door1;
    [SerializeField] GameObject door2;
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            door1.GetComponent<Animator>().SetTrigger("Open");
            door2.GetComponent<Animator>().SetTrigger("Open");
        }
    }
}
