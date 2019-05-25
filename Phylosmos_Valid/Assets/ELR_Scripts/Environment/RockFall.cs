using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFall : MonoBehaviour
{
    bool fallen = false;
    bool hasFallen = false;
    [SerializeField] AudioClip audioFall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().rockAb == true && hasFallen == false)
        {
            GetComponent<Animator>().SetTrigger("Fall");
            GetComponent<AudioSource>().clip = audioFall;
            GetComponent<AudioSource>().Play();
            fallen = true;
            hasFallen = true;
            StartCoroutine(StopFalling());
        }
        if(other.gameObject.tag == "Boss")
        {
            if(fallen == true)
            {
                other.gameObject.GetComponent<EnemyLife>().TakeDamage(30);
                fallen = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Spike" || other.gameObject.tag == "Liana" || other.gameObject.tag == "Healer" || other.gameObject.tag == "Rock" || other.gameObject.tag == "Boss")
        {
            if(fallen == true)
            {
                other.gameObject.GetComponent<EnemyLife>().TakeDamage(30);
            }
        }
    } 

    IEnumerator StopFalling()
    {
        yield return new WaitForSeconds(2);
        fallen = false;
    }

}
