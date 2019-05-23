using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] float grabForce;
    public GameObject player;
    GameObject anchor;
    bool checking = false;

    private void Start()
    {
        Destroy(this.gameObject, 2f);
    }
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Spike" || other.gameObject.tag == "Healer" || other.gameObject.tag == "Liana" || other.gameObject.tag == "Rock")
        {
            other.GetComponent<Rigidbody>().AddForce(-(other.gameObject.transform.position - player.transform.position) * grabForce);
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "GrabAnchor")
        {
            if(Vector3.Distance(other.gameObject.transform.position, player.transform.position) > 10)
            {
                player.GetComponent<CapsuleCollider>().isTrigger = true;
                player.GetComponent<Rigidbody>().useGravity = false;
                player.GetComponent<PlayerController>().currentState = PlayerState.Stagger;
                player.GetComponent<Rigidbody>().AddForce(-(player.transform.position - other.gameObject.transform.position) * grabForce * 1.5f);
                anchor = other.gameObject;
                StartCoroutine("Arrived");
            }
        }
        if(other.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }

    private void Update() 
    {
        if(anchor != null && Vector3.Distance(player.transform.position, anchor.transform.position) < 4f)
        {
            player.GetComponent<CapsuleCollider>().isTrigger = false;
            player.GetComponent<Rigidbody>().useGravity = true;
            player.GetComponent<PlayerController>().currentState = PlayerState.Idle;
        }
        if(checking)
        {
            player.GetComponent<CapsuleCollider>().isTrigger = false;
            player.GetComponent<Rigidbody>().useGravity = true;
            player.GetComponent<PlayerController>().currentState = PlayerState.Idle;
        }
        if(player.GetComponent<CapsuleCollider>().isTrigger == false && player.GetComponent<Rigidbody>().useGravity == true)
        {
            checking = false;
        }
        if(player.transform.position.y >= 20)
        {
            checking = true;
        }
    }
    IEnumerator Arrived ()
    {
        yield return new WaitForSeconds(3f);
        checking = true;
    }
}
