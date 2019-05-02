using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    [SerializeField] float grabForce;
    public GameObject lianaEnemy;

    private void Start()
    {
        Destroy(this.gameObject, 2f);
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<Rigidbody>().AddForce(-(other.gameObject.transform.position - lianaEnemy.transform.position) * grabForce);
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
