using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTreeFalling : MonoBehaviour
{
    bool fallen = false;
    bool hasFallen = false;
    [SerializeField] AudioClip audioFall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "PlayerGrab" && hasFallen == false)
        {
            GetComponent<Animator>().SetTrigger("Fall");
            GetComponent<AudioSource>().clip = audioFall;
            GetComponent<AudioSource>().Play();
            fallen = true;
            hasFallen = true;
            StartCoroutine(StopFalling());
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Spike" || other.gameObject.tag == "Liana" || other.gameObject.tag == "Healer" || other.gameObject.tag == "Rock" || other.gameObject.tag == "Boss")
        {
            if(fallen == true)
            {
                other.gameObject.GetComponent<EnemyLife>().TakeDamage(40);
            }
        }
    }

    IEnumerator StopFalling()
    {
        yield return new WaitForSeconds(2);
        fallen = false;
    }

}
