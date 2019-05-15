using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTrigger : MonoBehaviour
{

    [SerializeField] GameObject birds;

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            birds.GetComponent<ParticleSystem>().Play();
            Destroy(this.gameObject);
        }
    }
}
