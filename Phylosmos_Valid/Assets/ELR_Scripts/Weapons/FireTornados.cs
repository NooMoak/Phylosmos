using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTornados : MonoBehaviour
{
   private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerDamage>().TakeDamage(30);
        }
    }
}
